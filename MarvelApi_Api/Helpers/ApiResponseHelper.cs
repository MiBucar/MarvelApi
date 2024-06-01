using System.Net;
using MarvelApi_Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MarvelApi_Api.Helpers
{
    public class ApiResponseHelper
    {
        public ApiResponse GetApiReponseNotSuccess(string message)
        {
            var response = new ApiResponse
            {
                StatusCode = HttpStatusCode.InternalServerError,
                IsSuccess = false,
                ErrorMessages = new List<string> { message }
            };

            return response;
        }

        public ApiResponse GetApiResponseSuccess(object result, HttpStatusCode statusCode)
        {
            var response = new ApiResponse
            {
                Result = result,
                StatusCode = statusCode,
                IsSuccess = true
            };
            return response;
        }
    }
}