using sltlang.Common.AuthService.Dto;
using sltlang.Domain.Ports;

namespace sltlang.Domain.Logic
{
    public class UserLogic(IHttpService httpService) : IUserLogic
    {
        const string UserService = "AuthService";

        public async Task<UserDto?> GetUser(int userId)
        {
            var (success, response) = await httpService.JsonRequest<object, UserDto>(UserService, $"users/get/{userId}", [], HttpMethod.Get, null, TimeSpan.FromSeconds(5));
            return success ? response : null;
        }
    }
}
