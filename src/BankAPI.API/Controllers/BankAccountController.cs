using BankAPI.Application.Dtos;
using BankAPI.Application.Dtos.InputModels;
using BankAPI.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace BankAPI.API.Controllers
{
    [ApiController]
    [Route("bank-accounts")]
    public class BankAccountController : ControllerBase
    {
        private readonly IAddBankAccountService _addBankAccountService;
        private readonly IGetAllBankAccountService _getAllBankAccountService;

        public BankAccountController(
            IAddBankAccountService addBankAccountService,
            IGetAllBankAccountService getAllBankAccountService)
        {
            _addBankAccountService = addBankAccountService;
            _getAllBankAccountService = getAllBankAccountService;
        }

        [HttpPost("")]
        public async Task<IActionResult> AddBankAccount([FromBody] AddBankAccountInputModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(CustomApiResponse<object>.FromModelState(ModelState));
            }

            var response = await _addBankAccountService.AddAsync(inputModel);

            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAllBankAccounts(GetAllBankAccountInputModel inputModel)
        {
            var response = await _getAllBankAccountService.GetAllAsync(inputModel);

            return StatusCode(response.StatusCode, response);
        }
    }
}
