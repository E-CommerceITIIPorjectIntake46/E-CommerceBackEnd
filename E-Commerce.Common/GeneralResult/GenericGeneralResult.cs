using System.Text.Json.Serialization;

namespace E_Commerce.Common
{
    public class GenericGeneralResult<T> : GeneralResult
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public T? Data { get; set; }
        public static GenericGeneralResult<T> SuccessResult(T data, string message = "Success")
            => new() { Success = true, Message = message, Data = data, Errors = null };

        public static new GenericGeneralResult<T> SuccessResult(string message = "Success")
            => new() { Success = true, Message = message, Errors = null };
        public static new GenericGeneralResult<T> NotFound(string message = "Resource not found")
            => new() { Success = false, Message = message, Errors = null };

        public static new GenericGeneralResult<T> FailResult(string message = "Operation faild.")
            => new() { Success = false, Message = message, Errors = null };

        public static new GenericGeneralResult<T> FailResult(Dictionary<string, List<Error>> errors, string message = "One or more error occured")
            => new() { Success = false, Message = message, Errors = errors };
    }
}
