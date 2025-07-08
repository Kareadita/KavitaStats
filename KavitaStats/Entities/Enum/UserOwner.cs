namespace KavitaStats.Entities.Enum;

public enum UserOwner
{
    /**
     * Kavita has full control over the user
     */
    Native = 0,
    /**
     * The user is synced with the OIDC provider
     */
    OpenIdConnect = 1,
}