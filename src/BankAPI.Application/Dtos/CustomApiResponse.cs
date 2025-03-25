using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.Json.Serialization;

namespace BankAPI.Application.Dtos
{
    public class CustomApiResponse<T>
    {
        [JsonPropertyName("success")]
        public bool Success { get; }

        [JsonPropertyName("message")]
        public string Message { get; }

        [JsonPropertyName("data")]
        public T? Data { get; }

        [JsonPropertyName("status_code")]
        public int StatusCode { get; }

        [JsonPropertyName("errors")]
        public Dictionary<string, List<string>>? Errors { get; }

        protected CustomApiResponse(bool success, string message, T? data, int statusCode, Dictionary<string, List<string>>? errors = null)
        {
            Success = success;
            Message = message;
            Data = data;
            StatusCode = statusCode;
            Errors = errors;
        }

        public static CustomApiResponse<T> SuccessResponse(T data, string message = "Operação realizada com sucesso.", int statusCode = StatusCodes.Status200OK)
        {
            return new CustomApiResponse<T>(true, message, data, statusCode);
        }

        public static CustomApiResponse<T> FailResponse(string message, int statusCode = StatusCodes.Status400BadRequest, Dictionary<string, List<string>>? errors = null)
        {
            return new CustomApiResponse<T>(false, message, default, statusCode, errors);
        }

        public static CustomApiResponse<T> FromModelState(ModelStateDictionary modelState)
        {
            if (modelState == null)
            {
                throw new ArgumentNullException(nameof(modelState));
            }

            var errors = modelState
                .Where(ms => ms.Value?.Errors?.Any() == true)
                .ToDictionary(
                    ms => ms.Key,
                    ms => ms.Value!.Errors!.Select(e => e.ErrorMessage ?? string.Empty).ToList()
                );

            return FailResponse("Falha na validação dos dados.", StatusCodes.Status400BadRequest, errors);
        }
    }
}
