using ExaminationSystem.Common.Exceptions;
using System.Net;
using System.Text.Json;

namespace ExaminationSystem.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                // شغّل الطلب عادي (الـ Controller + Handler)
                await _next(context);
            }
            catch (NotFoundException ex)
            {
                // الحاجة مش موجودة → 404
                await WriteErrorResponse(context, HttpStatusCode.NotFound, ex.Message);
            }
            catch (ForbiddenException ex)
            {
                // مش من حقه → 403
                await WriteErrorResponse(context, HttpStatusCode.Forbidden, ex.Message);
            }
            catch (ConflictException ex)
            {
                // فيه تعارض → 409
                await WriteErrorResponse(context, HttpStatusCode.Conflict, ex.Message);
            }
            catch (Exception ex)
            {
                // أي حاجة تانية غير متوقعة → 500
                await WriteErrorResponse(context, HttpStatusCode.InternalServerError, "An unexpected error occurred.");
            }
        }

        // method مساعدة بتكتب الرد في شكل JSON
        private static async Task WriteErrorResponse(HttpContext context, HttpStatusCode statusCode, string message)
        {
            context.Response.StatusCode = (int)statusCode;
            context.Response.ContentType = "application/json";

            var response = new
            {
                status = (int)statusCode,
                message = message
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
