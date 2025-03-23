﻿using BankAPI.Application.Dtos;
using BankAPI.Application.Dtos.InputModels;
using BankAPI.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace BankAPI.API.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class BankAccountController : ControllerBase
    {
        private readonly IAddBankAccountService _addBankAccountService;

        public BankAccountController(IAddBankAccountService addBankAccountService)
        {
            _addBankAccountService = addBankAccountService;
        }

        [HttpPost]
        public async Task<IActionResult> AddBankAccount([FromBody] AddBankAccountInputModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(CustomApiResponse<object>.FromModelState(ModelState));
            }

            var response = await _addBankAccountService.AddAsync(inputModel);

            return StatusCode(response.StatusCode, response);
        }
    }
}
