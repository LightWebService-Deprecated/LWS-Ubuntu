using System.Diagnostics.CodeAnalysis;
using System.Text;
using LWSSandboxService.Model.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Rest;

namespace LWSSandboxService.Filter;

[ExcludeFromCodeCoverage]
public class RawJsonToObjectResult : IActionResult
{
    public string JsonString { get; set; }
    public int StatusCode { get; set; }

    public async Task ExecuteResultAsync(ActionContext context)
    {
        context.HttpContext.Response.StatusCode = StatusCode;
        context.HttpContext.Response.ContentType = "application/json; charset=utf-8";
        await using var streamWriter = new StreamWriter(context.HttpContext.Response.Body, Encoding.UTF8);
        await streamWriter.WriteAsync(JsonString);
        await streamWriter.FlushAsync();
    }
}

[ExcludeFromCodeCoverage]
public class CustomExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is HttpOperationException correctException)
        {
            context.Result = new RawJsonToObjectResult
            {
                JsonString = correctException.Response.Content,
                StatusCode = (int) correctException.Response.StatusCode
            };
        }
        else
        {
            var errorResponse = new ErrorResponse
            {
                StatusCodes = StatusCodes.Status500InternalServerError,
                Message = $"Unknown Error Occurred: {context.Exception.Message}",
                DetailedMessage = context.Exception.StackTrace,
                ErrorPath = context.HttpContext.Request.Path.Value
            };
            context.Result = new ObjectResult(errorResponse)
            {
                StatusCode = errorResponse.StatusCodes
            };
        }
    }
}