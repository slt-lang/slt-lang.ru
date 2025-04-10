using sltlang.Common.AuthService.Enums;
using System;
using System.Collections.Generic;

namespace sltlang.Common.AuthService.Dto
{
    public class UserDto : ShortUserDto
    {
        public Dictionary<Variable, object> Variables { get; set; } = default!;
        public Dictionary<Permission, PermissionDto> Permissions { get; set; } = default!;
        public DateTime RegistrationDate { get; set; }
    }
}
