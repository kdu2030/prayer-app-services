﻿using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace PrayerAppServices.Error {
    public class ErrorHandler {
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

        public static async Task HandleExceptionAsync(HttpContext context) {
            IExceptionHandlerPathFeature? exceptionHandlerPathFeature =
                context.Features.Get<IExceptionHandlerPathFeature>();
            Exception? exception = exceptionHandlerPathFeature?.Error;

            if (exception is ArgumentException) {
                await HandleArgumentExceptionAsync(context, exception as ArgumentException);
                return;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            Error error = new Error {
                ErrorCode = ErrorCode.GenericError,
                Message = exception?.Message ?? "An internal server error occurred.",
                Url = context.Request.Path,
                RequestMethod = context.Request.Method
            };
            await context.Response.WriteAsJsonAsync(error);
        }

        private static async Task HandleArgumentExceptionAsync(HttpContext context, ArgumentException exception) {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            DataValidationError error = new DataValidationError {
                ErrorCode = ErrorCode.DataValidationError,
                DataValidationErrors = [exception.Message],
                Message = "Data validation error",
                Url = context.Request.Path,
                RequestMethod = context.Request.Method
            };

            await context.Response.WriteAsJsonAsync(error);
        }
    }
}