using System;
using System.Collections.Generic;

namespace FluxoCaixaDiarioMicroservice.Presentation.Models
{
    public class ErrorResponse
    {
        public string Type { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public IEnumerable<string> Errors { get; set; }
        public int StatusCode { get; set; }
        public DateTime Timestamp { get; set; }

        public ErrorResponse()
        {
            Timestamp = DateTime.UtcNow;
        }

        public ErrorResponse(string message, int statusCode = 500) : this()
        {
            Message = message;
            StatusCode = statusCode;
            Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1";
        }

        public ErrorResponse(Exception ex, int statusCode = 500) : this()
        {
            Message = ex.Message;
            StackTrace = ex.StackTrace;
            StatusCode = statusCode;
            Type = ex.GetType().Name;
        }
    }
}
