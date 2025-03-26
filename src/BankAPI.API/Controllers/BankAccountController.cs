using BankAPI.Application.Dtos;
using BankAPI.Application.Dtos.InputModels;
using BankAPI.Application.Dtos.ViewModels;
using BankAPI.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

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
        [ProducesResponseType(typeof(CustomApiResponse<AddBankAccountViewModel>), StatusCodes.Status201Created, "application/json")]
        [ProducesResponseType(typeof(CustomApiResponse<string>), StatusCodes.Status409Conflict, "application/json")]
        [ProducesResponseType(typeof(CustomApiResponse<string>), StatusCodes.Status500InternalServerError, "application/json")]
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
        [ProducesResponseType(typeof(CustomApiResponse<IEnumerable<BankAccountViewModel>>), StatusCodes.Status200OK, "application/json")]
        [ProducesResponseType(typeof(CustomApiResponse<string>), StatusCodes.Status400BadRequest, "application/json")]
        public async Task<IActionResult> GetAllBankAccounts(GetAllBankAccountInputModel inputModel)
        {
            var response = await _getAllBankAccountService.GetAllAsync(inputModel);

            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("deactivate")]
        [ProducesResponseType(typeof(CustomApiResponse<DeactivateBankAccountViewModel>), StatusCodes.Status200OK, "application/json")]
        [ProducesResponseType(typeof(CustomApiResponse<string>), StatusCodes.Status400BadRequest, "application/json")]
        [ProducesResponseType(typeof(CustomApiResponse<string>), StatusCodes.Status500InternalServerError, "application/json")]
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
        [ProducesResponseType(typeof(CustomApiResponse<TransferBankAccountViewModel>), StatusCodes.Status200OK, "application/json")]
        [ProducesResponseType(typeof(CustomApiResponse<string>), StatusCodes.Status400BadRequest, "application/json")]
        [ProducesResponseType(typeof(CustomApiResponse<string>), StatusCodes.Status500InternalServerError, "application/json")]
        [SwaggerOperation(
            OperationId = "TransferirValor",
            Summary = "Realiza transferência entre contas bancárias",
            Description = @"Valida saldo, status das contas e registra a transação.
                            Exige autenticação básica.")]
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
