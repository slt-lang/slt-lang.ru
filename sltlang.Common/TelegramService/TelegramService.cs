using sltlang.Common.TelegramService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace sltlang.Common.TelegramService
{
    public class TelegramService(ITelegramHttpAdapter telegramHttpAdapter)
    {
        public const string TelegramServiceName = "TelegramService";

        public async Task Log(TelegramMessage telegramMessage)
        {
            try
            {
                await telegramHttpAdapter.SendBodyMessage(TelegramServiceName, "log", HttpMethod.Post, telegramMessage);
            }
            catch
            {

            }
        }

        public async Task Alert(TelegramMessage telegramMessage)
        {
            try
            {
                await telegramHttpAdapter.SendBodyMessage(TelegramServiceName, "alert", HttpMethod.Post, telegramMessage);
            }
            catch
            {

            }
        }
    }
}
