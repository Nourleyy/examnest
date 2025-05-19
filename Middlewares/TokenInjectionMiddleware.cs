namespace ExamNest.Middlewares
{
    public class TokenInjectionMiddleware
    {
        private readonly RequestDelegate _next;

        public TokenInjectionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var token = context.Request.Cookies["ExamNest.Token"];
            if (!string.IsNullOrEmpty(token))
            {
                context.Request.Headers.Authorization = "Bearer " + token;
            }

            await _next(context);
        }
    }

}
