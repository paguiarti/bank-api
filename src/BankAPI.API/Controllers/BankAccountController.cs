using BankAPI.Application.Dtos;
using BankAPI.Application.Dtos.InputModels;
using BankAPI.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace BankAPI.API.Controllers
{
    [ApiController]
    [Route("api/v1/bank-accounts")]
    public class BankAccountController : ControllerBase
    {
        private readonly IAddBankAccountService _addBankAccountService;
        private readonly IGetAllBankAccountService _getAllBankAccountService;
        private readonly IDeactivateBankAccountService _deactivateBankAccountService;
        private readonly ITransferBankAccountService _transferBankAccountService;

        public BankAccountController(
            IAddBankAccountService addBankAccountService,
            IGetAllBankAccountService getAllBankAccountService,
            IDeactivateBankAccountService deactivateBankAccountService,
            ITransferBankAccountService transferBankAccountService)
        {
            _addBankAccountService = addBankAccountService;
            _getAllBankAccountService = getAllBankAccountService;
            _deactivateBankAccountService = deactivateBankAccountService;
            _transferBankAccountService = transferBankAccountService;
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

        [HttpPut("deactivate")]
        public async Task<IActionResult> DeactivateBankAccount([FromBody] DeactivateBankAccountInputModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(CustomApiResponse<object>.FromModelState(ModelState));
            }

            var response = await _deactivateBankAccountService.DeactivateBankAccountAsync(inputModel);

            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("transfer")]
        public async Task<IActionResult> TransferBankAccount([FromBody] TransferBankAccountInputModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(CustomApiResponse<object>.FromModelState(ModelState));
            }

            var response = await _transferBankAccountService.TransferBankAccountAsync(inputModel);

            return StatusCode(response.StatusCode, response);
        }
    }
}
