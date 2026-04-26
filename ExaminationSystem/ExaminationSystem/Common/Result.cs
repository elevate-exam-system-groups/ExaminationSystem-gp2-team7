namespace ExaminationSystem.Common
{
    public class Result<T>
    {
        public bool IsSuccess { get; }
        public T? Data { get; }
        public string? Error { get; }
        public int StatusCode { get; }

        private Result(bool isSuccess, T? data, string? error, int statusCode)
        {
            IsSuccess = isSuccess;
            Data = data;
            Error = error;
            StatusCode = statusCode;
        }

        public static Result<T> Success(T data, int statusCode = StatusCodes.Status200OK) 
            => new(true, data, null, statusCode);

        public static Result<T> Failure(string error, int statusCode = StatusCodes.Status400BadRequest)
            => new(false, default, error, statusCode);

        public static Result<T> Created(T data, int statusCode = StatusCodes.Status201Created)
            => new(true, data, null, statusCode);

        public static Result<T> NotFound(string message, int statusCode = StatusCodes.Status404NotFound)
            => new(false, default, message, statusCode);

        public static Result<T> Conflict(string message, int statusCode = StatusCodes.Status409Conflict)
            => new(false, default, message, statusCode);

        public static Result<T> Unauthorized(string message, int statusCode = StatusCodes.Status401Unauthorized)
           => new(false, default, message, statusCode);

        // Overload عشان نرجع data مع الـ error (زي حالة 409 Conflict)
        public static Result<T> Failure(string error, int statusCode, T data)
            => new(false, data, error, statusCode);
    }
}
