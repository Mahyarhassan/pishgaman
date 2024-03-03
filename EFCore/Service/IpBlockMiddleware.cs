namespace EFCore.Service
{
    public class IpBlockMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly List<string> _blockedIpAddresses;

        public IpBlockMiddleware(RequestDelegate next, string blockedIps)
        {
            _next = next;
            _blockedIpAddresses = blockedIps.Split(';').ToList();
        }

        public async Task Invoke(HttpContext context)
        {
            var ipAddress = context.Connection.RemoteIpAddress?.MapToIPv4().ToString();//اگر از ل.کال هاست استفاده کنیم ایپی را تشخیص نمیدهد
            if (string.IsNullOrEmpty(ipAddress) || _blockedIpAddresses.Contains(ipAddress))
            {
                context.Response.StatusCode = 403; // Forbidden
                return;
            }
            await _next(context);
        }


    }


};
