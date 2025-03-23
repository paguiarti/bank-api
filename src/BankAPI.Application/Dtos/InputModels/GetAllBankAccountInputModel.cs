using Microsoft.AspNetCore.Mvc;

namespace BankAPI.Application.Dtos.InputModels
{
    public class GetAllBankAccountInputModel
    {
        [FromQuery(Name = "document")]
        public string? Document { get; set; }

        [FromQuery(Name = "name")]
        public string? Name { get; set; }

        [FromQuery(Name = "page")]
        public int Page { get; set; } = 1;

        private int _pageSize = 20;

        [FromQuery(Name = "page_size")]
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > 20 ? 20 : value;
        }
    }
}
