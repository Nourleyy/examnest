using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace ExamNest.Filters
{
    public class ApiResponse<T>
    {
        [JsonProperty("success")]
        public bool Success { get; set; } = false;

        [JsonProperty("message")]
        public string Message { get; set; } = string.Empty;

        [JsonProperty("data")]
        public T? Data { get; set; }

        [JsonProperty("errors")]
        public List<string>? Errors { get; set; } = new();
    }

    public class ApiResponseFilter : IActionFilter
    {


        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Result is ObjectResult objectResult &&
                objectResult.Value is not ApiResponse<object>)
            {
                var statusCode = objectResult.StatusCode ?? 200;
                var isSuccess = statusCode >= 200 && statusCode < 300;

                var wrapped = new ApiResponse<object>
                {
                    Success = isSuccess,
                    Data = isSuccess ? objectResult.Value : null,
                    Message = isSuccess ? "Success" : "Failed",
                    Errors = !isSuccess ? new List<string> { objectResult.Value?.ToString() ?? "An error occurred" } : null
                };

                context.Result = new ObjectResult(wrapped)
                {
                    StatusCode = statusCode
                };
            }

        }
        public void OnActionExecuting(ActionExecutingContext context)
        {

        }
    }
}
