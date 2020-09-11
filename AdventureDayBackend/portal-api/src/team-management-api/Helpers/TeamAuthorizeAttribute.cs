using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using team_management_api.Helpers;
using team_management_data;

namespace team_management_api.Helpers
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class TeamAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private readonly AuthorizationType[] allowedTeams;

        public TeamAuthorizeAttribute(params AuthorizationType[] teams)
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
                return;
            }

            var allowed = false;
            foreach (var t in allowedTeams)
            {
                if (t.Equals(AuthorizationType.AnyTeam))
                {
                    allowed = true;
                    break;
                }

                if (t == AuthorizationType.Admin && team.Name.Equals(AppSettings.AdminTeamName))
                {
                    allowed = true;
                    break;
                }

                if (t == AuthorizationType.OwnTeam)
                {
                    Console.WriteLine(context.HttpContext.Request.Headers);
                }
            }

            if (!allowed)
            {
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
        }
    }
}
