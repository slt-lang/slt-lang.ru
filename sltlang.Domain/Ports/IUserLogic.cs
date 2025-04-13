using sltlang.Common.AuthService.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sltlang.Domain.Ports
{
    public interface IUserLogic
    {
        Task<UserDto?> GetUser(int userId);
    }
}
