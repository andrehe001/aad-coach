namespace team_management_api.Data
{
    public interface IAuthenticationService
    {
        // JwtToken (teamId, username and IsAdmin Flag) Login(Username, Pwd)
        AuthenticateResponse Authenticate(AuthenticateRequest model);

        AuthenticateResponse AuthenticateAdmin(AuthenticateRequest model);
    }
}
