using System.Net;
using MarvelApi_Api.Helpers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MarvelApi_Api.ExceptionFilters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        ApiFilterResponseHelper _responseHelper;
        public GlobalExceptionFilter(ApiFilterResponseHelper responseHelper)
        {
            _responseHelper = responseHelper;
        }
        public void OnException(ExceptionContext context)
        {
            context.Result = _responseHelper.CreateErrorResponse(HttpStatusCode.InternalServerError, context.Exception.Message);
            context.ExceptionHandled = true;
        }
    }
}