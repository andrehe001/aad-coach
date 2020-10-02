namespace AdventureDay.PortalApi.Services
{
    public interface IAuthenticationService
    {
        // JwtToken (teamId, username and IsAdmin Flag) Login(Username, Pwd)
        AuthenticateResponse Authenticate(AuthenticateRequest model);

        AuthenticateResponse AuthenticateAdmin(AuthenticateRequest model);
    }
}
