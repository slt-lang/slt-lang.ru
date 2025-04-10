namespace sltlang.Common.AuthService.Enums
{
    /// <summary>
    /// Всегда ли передаётся эта переменная в данных авторизации
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Field, Inherited = false, AllowMultiple = true)]
    public sealed class AlwaysTranfsferAttribute : System.Attribute
    {
        public AlwaysTranfsferAttribute()
        {
        }
    }
}
