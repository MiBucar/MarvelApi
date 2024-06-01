using System.Net;
using MarvelApi_Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace MarvelApi_Api.Helpers
{
    public class ApiFilterResponseHelper
    {
        private readonly ILogger<ApiFilterResponseHelper> _logger;

        public ApiFilterResponseHelper(ILogger<ApiFilterResponseHelper> logger)
        {
            _logger = logger;
        }

        public JsonResult CreateErrorResponse(HttpStatusCode statusCode, string errorMessage)
        {
            _logger.LogError("Creating error response: {StatusCode} - {ErrorMessage}", statusCode, errorMessage);
            var response = new ApiResponse
            {
                IsSuccess = false,
                StatusCode = statusCode,
                ErrorMessages = new List<string> { errorMessage }
            };
            return new JsonResult(response) { StatusCode = (int)statusCode };
        }

        public JsonResult CreateErrorResponse(HttpStatusCode statusCode, List<string> errorMessages)
        {
            _logger.LogError("Creating error response: {StatusCode} - {ErrorMessages}", statusCode, string.Join(", ", errorMessages));
            var response = new ApiResponse
            {
                IsSuccess = false,
                StatusCode = statusCode,
                ErrorMessages = errorMessages
            };
            return new JsonResult(response) { StatusCode = (int)statusCode };
        }
    }
}