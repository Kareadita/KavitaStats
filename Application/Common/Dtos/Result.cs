using System.Text.Json.Serialization;

namespace Application.Common.Dtos
{
    public readonly struct Result
    {
        public bool Success { get; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Error { get; }

        private Result(bool success)
        {
            Success = success;
            Error = null;
        }

        private Result(string message)
        {
            Success = false;
            Error = message;
        }

        public static Result Ok() => new(true);
        public static Result Fail(string message) => new(message);
    }

    public readonly struct Result<T>
    {
        public bool Success { get; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Error { get; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public T Data { get; }

        private Result(T data)
        {
            Success = true;
            Error = null;
            Data = data;
        }

        private Result(string message)
        {
            Success = false;
            Error = message;
            Data = default;
        }

        public static Result<T> Ok(T data) => new(data);
        public static Result<T> Fail(string message) => new(message);
    }
}