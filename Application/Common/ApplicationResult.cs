namespace Application.Common
{
    public class ApplicationResult<T>
    {
        public bool IsSuccess { get; }
        public T? Data { get; }
        public string? ErrorCode { get; }

        private ApplicationResult(bool success, T? data, string? errorCode)
        {
            IsSuccess = success;
            Data = data;
            ErrorCode = errorCode;
        }

        public static ApplicationResult<T> Success(T data)
            => new(true, data, string.Empty);

        public static ApplicationResult<T> Failure(string errorCode)
            => new(false, default, errorCode);
    }

}
