using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using team_management_data;

namespace team_management_api.Controllers
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class TeamAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string[] allowedTeams;

        public TeamAuthorizeAttribute(params string[] teams)
        {
            this.allowedTeams = teams;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var team = (Team)context.HttpContext.Items["Team"];
            if (team == null)
            {
                // not logged in
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }

            if (!allowedTeams.Contains(team.Name))
            {
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
        }
    }
}
