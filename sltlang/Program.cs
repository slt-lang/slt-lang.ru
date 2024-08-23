using Specification;

namespace sltlang
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddOutputCache();
            builder.Services.AddSingleton<ILocaleService, LocaleService>();
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
                name: "specification",
                pattern: "{culture=ru}/specification/{article}", new { controller = "Specification", action = "Index" });

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
