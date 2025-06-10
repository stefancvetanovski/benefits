using System.Net;
using System.Text.Json;
using Api.Models;

namespace Api.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        HttpStatusCode statusCode;
        string message;

        switch (exception)
        {
            case ResourceNotFoundException:
                statusCode = HttpStatusCode.NotFound;
                message = exception.Message;
                break;

            default:
                // Default for unhandled exceptions
                statusCode = HttpStatusCode.InternalServerError;
                message = "An unexpected error occurred.";
                break;
        }

        // Set status code and response content
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;
        var jsonResponse = JsonSerializer.Serialize(new ApiResponse<string>
        {
            Success = false,
            Error = message
        });
        // Return the message as a JSON response
        return context.Response.WriteAsync(jsonResponse);
    }
}