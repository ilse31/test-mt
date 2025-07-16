namespace WebApplication1.Models
{
    public class ApiResponse
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public object? Data { get; set; }

        public ApiResponse(int statusCode, string? message = null, object? data = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessage(statusCode);
            Data = data;
        }

        public static ApiResponse Success(object data)
        {
            return new ApiResponse(200, "Success", data);
        }

        private static string? GetDefaultMessage(int statusCode)
        {
            return statusCode switch
            {
                200 => "Success",
                400 => "Bad request",
                401 => "Unauthorized",
                404 => "Resource not found",
                409 => "Conflict occurred",
                500 => "Internal server error",
                _ => null
            };
        }
    }
}
