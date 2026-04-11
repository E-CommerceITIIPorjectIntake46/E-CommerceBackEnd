using System.Text.Json.Serialization;

namespace E_Commerce.Common
{
    public class GeneralResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Dictionary<string, List<Error>>? Errors { get; set; }
        public static GeneralResult SuccessResult(string message = "Successs")
            => new() { Success = true, Message = message, Errors = null };

        public static GeneralResult NotFound(string message = "Resource not found")
            => new() { Success = false, Message = message, Errors = null };

        public static GeneralResult FailResult(string message = "Operation failed")
            => new() { Success = false, Message = message, Errors = null };

        public static GeneralResult FailResult(Dictionary<string, List<Error>> errors, string message = "One or more error occurred")
            => new() { Success = false, Message = message, Errors = errors };
    }
}
