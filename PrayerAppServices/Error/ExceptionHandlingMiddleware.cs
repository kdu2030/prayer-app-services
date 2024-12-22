using System.Net;

namespace PrayerAppServices.Error {
    public class ExceptionHandlingMiddleware(RequestDelegate next) {
        private readonly RequestDelegate _next = next;

        public async Task InvokeAsync(HttpContext context) {
            try {
                await _next(context);
            }
            catch (Exception exception) {
                HandleException(context, exception);
            }
        }

        private static void HandleException(HttpContext context, Exception exception) {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            Error error = new Error {
                ErrorCode = ErrorCode.GenericError,
                Message = "Internal server error",
                Url = context.Request.Path,
                RequestMethod = context.Request.Method
            };
            context.Response.WriteAsJsonAsync(error);
        }

    }
}
