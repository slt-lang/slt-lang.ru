namespace sltlang.Common.TelegramService.Models
{
    public class TelegramMessage
    {
        public string Message { get; set; } = default!;
        public string[] Tags { get; set; } = default!;
    }
}
