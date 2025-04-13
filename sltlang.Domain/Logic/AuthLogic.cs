using sltlang.Common.AuthService.Contracts;
using sltlang.Domain.Ports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sltlang.Domain.Logic
{
    public class AuthLogic(IHttpService httpService) : IAuthLogic
    {
        const string AuthService = "AuthService";

        public async Task<LoginResponse?> Login(LoginRequest loginRequest)
        {
            return await httpService.JsonRequest(AuthService, "login", [], HttpMethod.Post, loginRequest, async response =>
            {
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return new LoginResponse()
                    {
                        NeedRefresh = true
                    };
                }
                var result = await httpService.GetJsonContent<LoginResponse>(response);
                return response.IsSuccessStatusCode ? result : null;
            }, TimeSpan.FromSeconds(5));
        }

        public async Task<RegistrationResponse?> Registration(RegistrationRequest request)
        {
            var (success, response) = await httpService.JsonRequest<RegistrationRequest, RegistrationResponse>(AuthService, "register", [], HttpMethod.Post, request);
            return success ? response : null;
        }
    }
}
