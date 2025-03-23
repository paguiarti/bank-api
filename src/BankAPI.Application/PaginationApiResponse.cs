using BankAPI.Application.Dtos;
using System.Text.Json.Serialization;

namespace BankAPI.Application
{
    public class PaginationApiResponse<T> : CustomApiResponse<T>
    {
        public PaginationApiResponse(
            bool success, string message, T? data, int statusCode,
            int page, int pageSize, int totalCount,
            Dictionary<string, List<string>>? errors = null)
          : base(success, message, data, statusCode, errors)
        {
            Page = page;
            PageSize = pageSize;
            TotalCount = totalCount;
        }

        [JsonPropertyName("page")]
        public int Page { get; }

        [JsonPropertyName("page_size")]
        public int PageSize { get; }

        [JsonPropertyName("total_count")]
        public int TotalCount { get; }

        public static PaginationApiResponse<T> SuccessResponse(T data, int statusCode,
                                                      int page, int pageSize, int totalCount)
        {
            return new PaginationApiResponse<T>(true, string.Empty, data, statusCode, page, pageSize, totalCount);
        }

        public static PaginationApiResponse<T> FailResponse(string message, int statusCode,
                                                       Dictionary<string, List<string>>? errors = null)
        {
            return new PaginationApiResponse<T>(false, message, default, statusCode, 0, 0, 0, errors);
        }


    }
}
