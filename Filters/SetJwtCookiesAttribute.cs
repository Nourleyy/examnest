using ExamNest.DTO.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ExamNest.Filters
{
    public class SetJwtCookiesAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Result is ObjectResult objectResult && objectResult.Value is Tokens token)
            {
                var response = context.HttpContext.Response;

                response.Cookies.Append("ExamNest.Token", token.AccessToken, ClientCookiesOptions.Options);
                response.Cookies.Append("ExamNest.Refresh", token.RefreshToken, ClientCookiesOptions.RefreshTokenOptions);
            }

            base.OnActionExecuted(context);
        }
    }

}
