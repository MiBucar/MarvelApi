using System.Net;
using MarvelApi_Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace MarvelApi_Api.Helpers
{
    public class ApiFilterResponseHelper
    {
        public JsonResult CreateErrorResponse(HttpStatusCode statusCode, string errorMessage)
        {
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