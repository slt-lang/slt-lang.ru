using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace sltreserve.Controllers
{
    public class HomeController : Controller
    {
        private string Title(string culture)
        {
            switch (culture)
            {
                case "en": return "Error";
                default: return "Ошибка";
            }
        }
        private string ServerReloading(string culture)
        {
            switch (culture)
            {
                case "en": return "The server is currently offline. Most likely, it is being updated. It usually takes no more than a minute.";
                default: return "В данный момент сервер отключен. Скорее всего, он обновляется. Обычно это занимает не больше минуты.";
            }
        }

        [Route("Error/{statusCode}")]
        public IActionResult Error(int statusCode)
        {
            var feature = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            var c = feature.OriginalPath.Split("/", StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
            c = string.IsNullOrWhiteSpace(c) ? "ru" : c;
            ViewData["culture"] = c;
            ViewData["title"] = Title(c);
            ViewData["reloading"] = ServerReloading(c);
            return View(statusCode);
        }
    }
}
