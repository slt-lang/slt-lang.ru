namespace sltlang.Common.AuthService.Contracts
{
    public enum CreateInviteResult
    {
        TooManyInvites,
        InviteAlreadyExists,
        PermissionsError,
        Success,
        UnknownError,
    }
}
