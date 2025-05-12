using Newtonsoft.Json;

namespace ExamNest.Middlewares
{
    public class ErrorResponse
    {
        [JsonProperty("success")]
        public bool Success => false;
        [JsonProperty("message")]
        public string Message { get; set; } = string.Empty;
        [JsonProperty("errors")]
        public List<string>? Errors { get; set; }
    }

    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

        public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                var error = new ErrorResponse
                {
                    Message = "Something went wrong",
                    Errors = [ex.Message]
                };

                await context.Response.WriteAsJsonAsync(error);
            }
        }
    }
}
