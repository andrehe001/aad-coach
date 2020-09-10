using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace team_management_data
{
    public interface IAuthenticationService
    {
        // JwtToken (teamId, username and IsAdmin Flag) Login(Username, Pwd)
        AuthenticateResponse Authenticate(AuthenticateRequest model);

        AuthenticateResponse AuthenticateAdmin(AuthenticateRequest model);
    }
}
