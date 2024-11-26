using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using FluxoCaixaDiarioMicroservice.Presentation.Exceptions;
using FluxoCaixaDiarioMicroservice.Presentation.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace FluxoCaixaDiarioMicroservice.Presentation.Middleware
{
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

        public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro não tratado: {Message}", ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            var response = new ErrorResponse();

            switch (exception)
            {
                case LancamentoNotFoundException ex:
                    response = new ErrorResponse(ex, StatusCodes.Status404NotFound);
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                    break;

                case ArgumentNullException ex:
                    response = new ErrorResponse(ex, StatusCodes.Status400BadRequest);
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    break;

                case InvalidOperationException ex:
                    response = new ErrorResponse(ex, StatusCodes.Status400BadRequest);
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    break;

                default:
                    response = new ErrorResponse(
                        "Ocorreu um erro interno no servidor. Por favor, tente novamente mais tarde.",
                        StatusCodes.Status500InternalServerError);
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    break;
            }

            var result = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await context.Response.WriteAsync(result);
        }
    }
}
