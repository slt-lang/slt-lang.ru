using sltlang.Common.TelegramService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sltlang.Common.TelegramService
{
    public static class TelegramServiceExtensions
    {
        public static void FireForgetLog(this TelegramService telegramService, TelegramMessage telegramMessage)
        {
            Task.Run(async () => await telegramService.Log(telegramMessage));
        }

        public static void FireForgetAlert(this TelegramService telegramService, TelegramMessage telegramMessage)
        {
            Task.Run(async () => await telegramService.Alert(telegramMessage));
        }
    }
}
