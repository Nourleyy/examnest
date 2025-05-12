using ExamNest.Middlewares;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace ExamNest.Filters
{
    public class ApiResponse<T>
    {
        [JsonProperty("success")]
        public bool Success { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; } = string.Empty;
        [JsonProperty("data")]
        public T? Data { get; set; }
    }
    public class ApiResponseFilter : IActionFilter
    {


        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Result is ObjectResult objectResult &&
                objectResult.Value is not ApiResponse<object> &&
                objectResult.Value is not ErrorResponse)
            {
                var wrapped = new ApiResponse<object>
                {
                    Success = objectResult.StatusCode >= 200 && objectResult.StatusCode < 300,
                    Data = objectResult.Value,
                    Message = "Success"
                };

                context.Result = new ObjectResult(wrapped)
                {
                    StatusCode = objectResult.StatusCode
                };
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {

        }
    }

}
