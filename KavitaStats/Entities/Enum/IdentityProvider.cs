using System.ComponentModel;

namespace KavitaStats.Entities.Enum;

public enum IdentityProvider
{
    [Description("Kavita")]
    Kavita = 0,
    [Description("OpenID Connect")]
    OpenIdConnect = 1,
}