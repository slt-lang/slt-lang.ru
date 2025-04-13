using sltlang.Common.AuthService.Enums;
using System;

namespace sltlang.Common.AuthService.Dto
{
    public class PermissionDto
    {
        public int Id { get; set; }
        public Permission Permission { get; set; }
        public bool AllowInheritance { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
