using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SLThree;
using SLThree.Extensions;
using SLThree.Metadata;
using sltlang.Adapters.Adapters;
using sltlang.Common.TelegramService;
using sltlang.Domain;
using sltlang.Domain.Logic;
using sltlang.Domain.Ports;
using Specification;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace sltlang
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddOutputCache();
            builder.Services.AddTransient<ILanguageProvider, Metadata>();
            builder.Services.AddSingleton<ISampleMaker, SampleMaker>();
            builder.Services.AddSingleton<ISampleStorage, SampleStorage>();
            builder.Services.AddTransient<ISampleLogic, SampleLogic>();
            builder.Services.AddTransient<IArticleLogic, ArticleLogic>();
            builder.Services.AddTransient<IAuthLogic, AuthLogic>();
            builder.Services.AddTransient<IHttpService, HttpService>();
            builder.Services.AddSingleton<ILocaleService, LocaleService>();
            builder.Services.AddSingleton<ISyntaxPageMaker, SyntaxPageMaker>();
            builder.Services.AddSingleton<ISyntaxPageStorage, SyntaxPageStorage>();
            builder.Services.AddTransient<SLThreeHtmlRestorator>();

            builder.Services.AddTransient<ITelegramHttpAdapter, HttpService>();
            builder.Services.AddTransient<TelegramService>();

            #region Rate-Limit

            builder.Services.AddMemoryCache();

            builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));

            builder.Services.AddInMemoryRateLimiting();
            builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

            #endregion

            builder.Services.AddControllersWithViews();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "slt-lang.ru API", Version = typeof(Program).Assembly.GetName().Version!.ToString(3), 
                    Description = "По умолчанию, будет использована авторизация, благодаря которой получен доступ к Swagger. Но вы также можете использовать Bearer-заголовки (они приоритетнее)."});

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Token for Bearer auth",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT"
                });

                options.OperationFilter<AuthorizeCheckOperationFilter>();
            });

            var configuration = builder.Configuration.GetSection("Config").Get<Config>();
            builder.Services.AddSingleton(configuration!);

            foreach (var peer in configuration!.PeerServices)
            {
                builder.Services.AddHttpClient(peer.Key, clientcfg =>
                {
                    clientcfg.BaseAddress = new Uri(peer.Value!);
                });
            }

            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // Adjust as needed
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            #region Авторизация
            EncodingProvider provider = CodePagesEncodingProvider.Instance;
            Encoding.RegisterProvider(provider);

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration.JwtSettings.Issuer,
                        ValidAudience = configuration.JwtSettings.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.JwtSettings.Secret))
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var authorization = context.Request.Headers.Authorization.FirstOrDefault();

                            if (!string.IsNullOrEmpty(authorization) && authorization.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                            {
                                context.Token = authorization.Substring("Bearer ".Length).Trim();
                                return Task.CompletedTask;
                            }

                            var token = context.Request.Cookies["JwtToken"];
                            if (!string.IsNullOrEmpty(token))
                            {
                                context.Token = token;
                                return Task.CompletedTask;
                            }

                            context.Fail("No token found in headers or cookies.");
                            return Task.CompletedTask;
                        },
                        OnChallenge = context =>
                        {
                            context.HandleResponse();

                            if (!context.HttpContext.User.Identity?.IsAuthenticated ?? true)
                            {
                                context.HttpContext.Response.Redirect("/Login");
                                return Task.CompletedTask;
                            }


                            if (context.AuthenticateFailure != null)
                            {
                                context.HttpContext.Response.Cookies.Delete("JwtToken");
                                context.HttpContext.Response.Redirect("/Login");
                            }


                            return Task.CompletedTask;
                        }
                    };
                });
            builder.Services.AddAuthorization();
            #endregion

            var app = builder.Build();

            app.UseSwagger();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseOutputCache();
            app.UseIpRateLimiting();
            app.UseRouting();

            app.UseSession();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<SwaggerAuthorizationMiddleware>("/swagger");
            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = "swagger";
            });

            app.UseStatusCodePagesWithReExecute("/Error/{0}");

            app.MapControllerRoute(
                name: "api", 
                pattern: "api");

            app.MapControllerRoute(
                name: "home_not_culture",
                pattern: "{action=Index}", new { controller = "Home" });

            app.MapControllerRoute(
                name: "home",
                pattern: "{culture=ru}/{action=Index}", new { controller = "Home" }, new { culture = "^(?!api$).*$" });

            app.MapControllerRoute(
                name: "syntax",
                pattern: "{culture=ru}/syntax/{article}", new { controller = "Syntax", action = "Index" }, new { culture = "^(?!api$).*$" });

            app.MapControllerRoute(
                name: "articles",
                pattern: "{culture=ru}/articles/{article}", new { controller = "Article", action = "Index" }, new { culture = "^(?!api$).*$" });

            app.MapControllerRoute(
                name: "profile",
                pattern: "{culture=ru}/profile/{action=Index}", new { controller = "Profile" }, new { culture = "^(?!api$).*$" });

            app.Run();
        }
    }
}
