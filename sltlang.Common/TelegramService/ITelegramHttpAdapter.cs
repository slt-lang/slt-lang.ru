using sltlang.Common.TelegramService.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace sltlang.Common.TelegramService
{
    public interface ITelegramHttpAdapter
    {
        Task SendBodyMessage(string serviceName, string path, HttpMethod httpMethod, TelegramMessage telegramMessage);
    }
}
