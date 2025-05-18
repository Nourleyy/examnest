using ExamNest.Errors;
using ExamNest.Filters;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Net;

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
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, "Database error occurred");
                await HandleSqlException(context, sqlEx);
            }
            catch (InvalidOperationException opEx)
            {
                _logger.LogError(opEx, "Invalid operation error");
                await HandleInvalidOperationException(context, opEx);
            }
            catch (ResourceDeleteException ex)
            {
                _logger.LogError(ex, "ResourceDeleteException");

                await HandleResourceDeleteException(context, ex);

            }
            catch (ResourceNotFoundException ex)
            {

                _logger.LogError(ex, "ResourceNotFoundException");
                await HandleResourceNotFoundException(context, ex);
            }
            catch (ResourceAlreadyExistsException ex)
            {
                _logger.LogError(ex, "ResourceAlreadyExistsException");
                await HandleGenericException(context, ex);

            }catch (UnauthorizedAccessException ex)
            {
                _logger.LogError(ex, "HandleUnauthorizedException");
                await HandleUnauthorizedException(context, ex);

            }
            catch (BadHttpRequestException ex)
            {
                _logger.LogError(ex, "BadHttpRequestException");
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Response.ContentType = "application/json";
                var error = new ApiResponse<object>
                {
                    Message = "Bad Request",
                    Errors = ex.Message.Split(",").ToList()
                };
                await context.Response.WriteAsJsonAsync(error);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred");
                await HandleGenericException(context, ex);
            }
        }
        private Task HandleResourceNotFoundException(HttpContext context, ResourceNotFoundException ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            var error = new ApiResponse<object>
            {
                Message = "Resource Not Found",
                Errors = new List<string> { ex.Message }

            };

            return context.Response.WriteAsJsonAsync(error);
        }

        private Task HandleUnauthorizedException(HttpContext context, UnauthorizedAccessException ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;

            var error = new ApiResponse<object>
            {

                Message = "Unauthorized",
                Errors = new List<string> { ex.Message }
            };
            return context.Response.WriteAsJsonAsync(error);


        }
        private Task HandleResourceDeleteException(HttpContext context, ResourceDeleteException ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            var error = new ApiResponse<object>
            {
                Message = "Error while deleting",
                Errors = new List<string> { ex.Message }
            };
            return context.Response.WriteAsJsonAsync(error);
        }

        private async Task HandleSqlException(HttpContext context, SqlException ex)
        {
            context.Response.ContentType = "application/json";

            var error = new ApiResponse<object>
            {
                Message = DetermineSqlErrorMessage(ex),
                Errors = new List<string> { ex.Message }
            };

            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            await context.Response.WriteAsJsonAsync(error);
        }

        private string DetermineSqlErrorMessage(SqlException ex)
        {
            return ex.Number switch
            {
                547 => "Cannot delete due to existing dependencies.",
                2627 => "A duplicate record already exists.",
                _ => "A database error occurred."
            };
        }

        private async Task HandleInvalidOperationException(HttpContext context, InvalidOperationException ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            var error = new ApiResponse<object>
            {
                Message = "Invalid operation",
                Errors = new List<string> { ex.Message }
            };

            await context.Response.WriteAsJsonAsync(error);
        }

        private async Task HandleGenericException(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var error = new ApiResponse<object>
            {
                Message = "An unexpected error occurred",
                Errors = new List<string> { ex.Message }
            };

            await context.Response.WriteAsJsonAsync(error);
        }
    }
}
