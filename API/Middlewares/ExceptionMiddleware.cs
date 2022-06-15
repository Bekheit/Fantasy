using API.Errors;
using System.Net;
using System.Text.Json;

namespace API.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IHostEnvironment env;

        public ExceptionMiddleware(RequestDelegate next, IHostEnvironment env)
        {
            this.next = next;
            this.env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            Stream originalBody = context.Response.Body;
            try
            {
                using (var memStream = new MemoryStream())
                {
                    context.Response.Body = memStream;

                    await next(context);

                    memStream.Position = 0;
                    string responseBody = new StreamReader(memStream).ReadToEnd();
                    
                    memStream.Position = 0;
                    await memStream.CopyToAsync(originalBody);
                }
            }
            catch (Exception ex)
            {

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var response = env.IsDevelopment()
                    ? new ApiException(context.Response.StatusCode, ex.Message, ex.StackTrace?.ToString())
                    : new ApiException(context.Response.StatusCode, "Internal Server Error");

                var json = JsonSerializer.Serialize(response);

                await context.Response.WriteAsync(json);
            }
            finally
            {
                context.Response.Body = originalBody;
            }
        }
    }
}
