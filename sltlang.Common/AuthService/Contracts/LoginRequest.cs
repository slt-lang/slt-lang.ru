namespace sltlang.Common.AuthService.Contracts
{
    public class LoginRequest
    {
        public string Login { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string RedirectUrl { get; set; } = default!;
    }

    public class LoginResponse
    {
        public string AccessToken { get; set; } = default!;
        public bool NeedRefresh { get; set; } = false;
    }
}
