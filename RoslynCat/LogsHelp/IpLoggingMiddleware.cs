namespace RoslynCat.Logs
{
    public class IpLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public IpLoggingMiddleware(RequestDelegate next) {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context) {
            // 获取用户的 IP 地址
            string ipAddress = context.Connection.RemoteIpAddress?.ToString();

            // 记录用户的 IP 地址
            Log(ipAddress);

            // 将请求传递给下一个中间件
            await _next(context);
        }

        private void Log(string ipAddress) {
            // 在这里记录用户 IP 地址的日志
            Console.WriteLine($"User IP address: {ipAddress}");
        }
    }
}