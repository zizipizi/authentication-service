using System;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.Host.Results
{
    public struct Result<TResult> where TResult : Enum
    {
        public TResult Value { get; }
         
        public string Message { get; }

        public Result(TResult value, string message = "")
        {
            Value = value;
            Message = message;
        }
    }

    public struct Result<TResult, TModel> where TResult : Enum
    {
        public TResult Value { get; }

        public TModel Model { get; }

        public string Message { get; }

        public Result(TResult value, TModel model = default, string message = "")
        {
            Value = value;
            Message = message;
            Model =  model;
        }
    }


    public struct WebApiResult<TModel> where TModel : class
    {
        public HttpStatusCode StatusCode { get; }

        public TModel Model { get; }

        public string Message { get; }

        public WebApiResult(HttpStatusCode statusCode, TModel model = null, string message = "")
        {
            StatusCode = statusCode;
            Model = model;
            Message = message;
        }
    }

    public static class ResultExtensions 
    {
        // Not work if i set number of enum manually
        public static bool IsEquals<T>(this Result<T> result, T resultEnum) where T : Enum
        {
            if (result.Value.Equals(resultEnum))
                return true;

            return false;
        }

        public static bool IsEquals<Tr, Tm>(this Result<Tr, Tm> result, Tr resultEnum) where Tr : Enum where Tm : class
        {
            if (result.Value.Equals(resultEnum))
                return true;

            return false;
        }

        public static Result<T> Result<T>(T result, string message = "") where T : Enum
        {
            return new Result<T>(result, message);
        }

        public static Result<Te, Tm> Result<Te, Tm>(Te result, Tm model, string message = "") where Te : Enum where Tm : class 
        {
            return new Result<Te, Tm>(result, model, message);
        }

        public static IActionResult ToActionResult<T>(this Result<HttpStatusCode, T> result) where T : class
        {
            return result.Value switch
            {
                HttpStatusCode.OK => result.Model == null
                    ? new OkResult()
                    : new OkObjectResult(result.Model) as IActionResult,
                HttpStatusCode.Unauthorized => string.IsNullOrEmpty(result.Message)
                    ? new UnauthorizedResult()
                    : new UnauthorizedObjectResult(result.Message) as IActionResult,
                HttpStatusCode.NotFound => string.IsNullOrEmpty(result.Message)
                    ? new NotFoundResult()
                    : new NotFoundObjectResult(result.Message) as IActionResult,
                HttpStatusCode.NoContent => string.IsNullOrEmpty(result.Message)
                    ? new NoContentResult()
                    : new ObjectResult(result.Message) { StatusCode = StatusCodes.Status204NoContent } as IActionResult,
                HttpStatusCode.ServiceUnavailable => string.IsNullOrEmpty(result.Message)
                    ? new StatusCodeResult(StatusCodes.Status503ServiceUnavailable)
                    : new ObjectResult(result.Message)
                        { StatusCode = StatusCodes.Status503ServiceUnavailable } as IActionResult,
                HttpStatusCode.Conflict => string.IsNullOrEmpty(result.Message)
                    ? new ConflictResult()
                    : new ConflictObjectResult(result.Message) as IActionResult,

                _ => new BadRequestObjectResult(result.Message)
            };
        }

        public static IActionResult ToActionResult(this Result<HttpStatusCode> result)
        {
            return result.Value switch
            {
                HttpStatusCode.OK => string.IsNullOrEmpty(result.Message)
                    ? new OkResult()
                    : new OkObjectResult(result.Message) as IActionResult,
                HttpStatusCode.Unauthorized => string.IsNullOrEmpty(result.Message)
                    ? new UnauthorizedResult()
                    : new UnauthorizedObjectResult(result.Message) as IActionResult,
                HttpStatusCode.NotFound => string.IsNullOrEmpty(result.Message)
                    ? new NotFoundResult()
                    : new NotFoundObjectResult(result.Message) as IActionResult,
                HttpStatusCode.NoContent => string.IsNullOrEmpty(result.Message)
                    ? new NoContentResult()
                    : new ObjectResult(result.Message) {StatusCode = StatusCodes.Status204NoContent} as IActionResult,
                HttpStatusCode.ServiceUnavailable => string.IsNullOrEmpty(result.Message)
                    ? new StatusCodeResult(StatusCodes.Status503ServiceUnavailable)
                    : new ObjectResult(result.Message)
                        {StatusCode = StatusCodes.Status503ServiceUnavailable} as IActionResult,
                HttpStatusCode.Conflict => string.IsNullOrEmpty(result.Message)
                    ? new ConflictResult()
                    : new ConflictObjectResult(result.Message) as IActionResult,

                _ => new BadRequestObjectResult(result.Message)
            };
        }
    }
}
