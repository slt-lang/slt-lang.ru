using sltlang.Common.AuthService.Contracts;

namespace sltlang.Domain.Ports
{
    public interface IAuthLogic
    {
        Task<LoginResponse?> Login(LoginRequest loginRequest);
    }
}
