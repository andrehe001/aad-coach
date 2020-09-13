using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using team_management_api.Data;

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
                // attenion ugly code ahead ;)
                if (t == AuthorizationType.OwnTeam)
                {
                    string url = context.HttpContext.Request.GetDisplayUrl();
                    if (url.EndsWith("/api/allwithmembers") || url.EndsWith("/api/all"))
                    {
                        allowed = true;
                        break;
                    }
                    var paths = url.Split("/");
                    bool isController = false;
                    bool isAction = false;
                    bool isTeamId = false;
                    int teamId = 0;
                    foreach (var path in paths)
                    {
                        if (path.Equals("api"))
                        {
                            isController = true;
                            continue;
                        }
                        if (isController)
                        {
                            isAction = true;
                            isController = false;
                            continue;
                        }
                        if (isAction)
                        {
                            isTeamId = true;
                            isAction = false;
                            continue;
                        }
                        if (isTeamId)
                        {
                            if (int.TryParse(path, out teamId))
                            {
                                break;
                            }
                            else
                            {
                                continue;
                            }
                        }
                    }

                    if (teamId > 0 && teamId == team.Id)
                    {
                        allowed = true;
                        break;
                    }
                }
            }

            if (!allowed)
            {
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
        }
    }
}
