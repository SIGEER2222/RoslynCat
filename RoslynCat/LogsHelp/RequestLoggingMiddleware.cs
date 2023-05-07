namespace RoslynCat.Logs
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestLoggingMiddleware(RequestDelegate next) {
            _next = next;
        }

        public async Task Invoke(HttpContext context) {
            var ipAddress = context.Connection.RemoteIpAddress.ToString();
            var requestPath = context.Request.Path;

            // 在这里记录IP地址和请求路径
            Console.WriteLine($"IP Address: {ipAddress}, Request Path: {requestPath}");

            await _next(context);

        }
    }
}
