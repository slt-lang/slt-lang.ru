using SLThree;
using SLThree.Metadata;
using sltlang.Adapters.Adapters;
using sltlang.Domain.Logic;
using sltlang.Domain.Ports;
using Specification;

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
            builder.Services.AddSingleton<ILocaleService, LocaleService>();
            builder.Services.AddSingleton<ISyntaxPageMaker, SyntaxPageMaker>();
            builder.Services.AddSingleton<ISyntaxPageStorage, SyntaxPageStorage>();
            builder.Services.AddTransient<SLThreeHtmlRestorator>();
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

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
            app.UseRouting();

            app.UseAuthorization();

            app.UseStatusCodePagesWithReExecute("/Error/{0}");

            app.MapControllerRoute(
                name: "syntax",
                pattern: "{culture=ru}/syntax/{article}", new { controller = "Syntax", action = "Index" });

            app.MapControllerRoute(
                name: "articles",
                pattern: "{culture=ru}/other/{article}", new { controller = "Article", action = "Index" });

            app.MapControllerRoute(
                name: "default",
                pattern: "{culture=ru}/{action=Index}/", new { controller = "Home" });

            app.Run();
        }
    }
}
