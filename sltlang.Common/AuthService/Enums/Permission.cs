using System.ComponentModel;
using System.Text.Json.Serialization;

namespace sltlang.Common.AuthService.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Permission
    {
        [Description("Root")]
        RootPermission = 1024,

        [Description("Смена имени пользователя")]
        AuthUsernameChanging = 53000,

        [Description("Смена пароля")]
        AuthPasswordChanging,

        [Description("Ссылки-приглашения")]
        AuthInviteLinks,

        [Description("Многоразовые ссылки-приглашения")]
        AuthReusableInviteLinks,

        [Description("Наследование прав по ссылкам-приглашениям")]
        AuthInheritanceInviteLinks,

        [Description("Изменение переменных (Security None)")]
        AuthChangingNoneSecureVariables,

        [Description("Изменение переменных (Security Low)")]
        AuthChangingLowSecureVariables,

        [Description("Изменение переменных (Security Medium)")]
        AuthChangingMediumSecureVariables,

        [Description("Изменение переменных (Security High)")]
        AuthChangingHighSecureVariables,

        [Description("Изменение переменных (Security Critical)")]
        AuthChangingCriticalSecureVariables,

        //--------------
        //-- [СТАТЬИ] --
        //--------------
        [Description("[Статьи] Создание новых")]
        ArticleCreating = 52000,

        [Description("[Статьи] Внесение изменений")]
        ArticleEditing,

        [Description("[Статьи] Закрепление")]
        ArticlePinning,

        [Description("[Статьи] Удаление")]
        ArticleRemoving,
    }
}
