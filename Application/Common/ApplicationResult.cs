namespace Application.Common
{
    public class ApplicationResult<T>
    {
        public bool IsSuccess { get; }
        public T? Data { get; }
        public int ErrorCode { get; }

        private ApplicationResult(bool success, T? data, int errorCode)
        {
            IsSuccess = success;
            Data = data;
            ErrorCode = errorCode;
        }

        public static ApplicationResult<T> Success(T data)
            => new(true, data, default);

        public static ApplicationResult<T> Failure(int errorCode)
            => new(false, default, errorCode);
    }

}
