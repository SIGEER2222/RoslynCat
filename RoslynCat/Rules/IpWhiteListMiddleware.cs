using System.Net;

namespace RoslynCat.Rules
{
    public class IpWhiteListMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<IpWhiteListMiddleware> _logger;
        private readonly IConfiguration _config;
        private readonly string[] _allowedIpAddresses;

        public IpWhiteListMiddleware(RequestDelegate next, ILogger<IpWhiteListMiddleware> logger, IConfiguration config)
        {
            _next = next;
            _logger = logger;
            _config = config;
            _allowedIpAddresses = _config["AdminSafeList"].Split(";");
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments("/test")) {
                var remoteIpAddress = context.Connection.RemoteIpAddress;

                if (!IsIpAddressAllowed(remoteIpAddress))
                {
                    _logger.LogWarning("Access denied to {IpAddress}", remoteIpAddress);

                    context.Response.StatusCode = 403;
                    await context.Response.WriteAsync("这不是你该来的地方");
                    return;
                }
            }
            await _next(context);
        }

        private bool IsIpAddressAllowed(IPAddress ipAddress)
        {
            return _allowedIpAddresses.Any(ip => IPAddress.Parse(ip).Equals(ipAddress));
        }
    }
}