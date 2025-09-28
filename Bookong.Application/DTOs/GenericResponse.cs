namespace Bookong.Application.DTOs
{
    public class GenericResponse
    {
        public bool Success { get; protected set; }

        public string Message { get; protected set; } = "";

        public string? StatusCode { get; protected set; }

        public object? Payload { get; protected set; }

        protected GenericResponse(bool success, string message, string? statusCode, object? payload)
        {
            Success = success;
            Message = message;
            StatusCode = statusCode;
            Payload = payload;
        }

        public static GenericResponse SuccessResponse(string message = "", object? payload = null)
        {
            return new GenericResponse(true, message, null, payload);
        }

        public static GenericResponse FailureResponse(string message = "", string? statusCode = null, object? payload = null)
        {
            return new GenericResponse(false, message, statusCode, payload);
        }
    }
}
