using Microsoft.AspNetCore.Mvc;

namespace PrayerAppServices.Error {
    public class DataValidationHandler {
        public static IActionResult HandleDataValidationErrors(ActionContext context) {
            IEnumerable<string> errorMessages = context.ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage);
            DataValidationError error = new DataValidationError {
                ErrorCode = ErrorCode.DataValidationError,
                DataValidationErrors = errorMessages,
                Message = "Data validation error",
                Url = context.HttpContext.Request.Path,
                RequestMethod = context.HttpContext.Request.Method
            };
            return new BadRequestObjectResult(error);
        }
    }
}