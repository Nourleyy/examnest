using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace ExamNest.Filters
{
    public class ApiResponse<T>
    {
        [JsonProperty("success")] public bool Success { get; set; } = false;
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
                objectResult.Value is not ApiResponse<object>
                )
            {
                var wrapped = new ApiResponse<object>
                {
                    Success = objectResult.StatusCode >= 200 && objectResult.StatusCode < 300,
                    Data = objectResult.StatusCode <= 200 ? objectResult.Value : null,
                    Message = objectResult.StatusCode >= 400 ? "failed" : "sucsuss",
                    Errors = objectResult.StatusCode >= 400 ? new List<string> { objectResult.Value?.ToString() ?? "An error occurred" } : null
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
