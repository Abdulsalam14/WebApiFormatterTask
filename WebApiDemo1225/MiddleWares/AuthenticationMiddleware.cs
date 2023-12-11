using System.Net;
using System.Security.Claims;
using System.Text;

namespace WebApiDemo1225.MiddleWares
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {

            // yoxlayin ki eger SignIn action methoddusa
            // onda 
             /////       await _next(context);
             ///

            string authHeader = context.Request.Headers["Authorization"];
            if (authHeader == null || authHeader.Trim()=="")
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }

            //basic elvin:12345
            if (authHeader != null && authHeader.StartsWith("basic", StringComparison.OrdinalIgnoreCase))
            {
                var token = authHeader.Substring(6).Trim();
                string credentialString = "";
                try
                {
                    credentialString = Encoding.UTF8.GetString(Convert.FromBase64String(token));
                }
                catch (Exception)
                {
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    return;
                }

                var credentials = credentialString.Split(':');

                var username = credentials[0];
                var password = credentials[1];

                if (username == "elvin123" && password == "12345")// for testing purpose
                {
                    var claims = new[]
                    {
                        new Claim("username",username),
                        new Claim(ClaimTypes.Role,"Admin")
                    };

                    var identity = new ClaimsIdentity(claims, "Basic");

                    context.User = new ClaimsPrincipal(identity);
                    await _next(context);
                }
                else
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                }
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            }
        }
    }
}
