using EasyPay.Api.Models.Transfers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using EasyPay.Api.Services.Foundations.Transfers;
using EasyPay.Api.Models.Accounts.Exceptions;

namespace EasyPay.Api.Controllers
{
    public class TransferController : Controller
    {
        private readonly ITransferService transferService;

        public TransferController(ITransferService transferService)
        {
            this.transferService = transferService;
        }

        [HttpPost("transfer")]
        public async Task<ActionResult> Transfer(string sourceAccountNumber, string receiverAccountNumber, decimal amount)
        {
            try
            {
                var updatedBalance = await this.transferService
                    .MakeTransferAsync(sourceAccountNumber, receiverAccountNumber, amount);

                return Ok(updatedBalance);
            }
            catch (AccountValidationException accountValidationException)
            {
                return BadRequest(accountValidationException.InnerException.Message);
            }
        }

        [HttpPost("deposit")]
        public async ValueTask<IActionResult> Deposit(string accountNumber, decimal amount)
        {
            try
            {
                var updatedBalance = await transferService.DepositAsync(accountNumber, amount);

                return Ok(updatedBalance);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("balance")]
        public async Task<IActionResult> GetBalance(string accountNumber)
        {
            try
            {
                var balance = await transferService.CheckBalanceAsync(accountNumber);
                return Ok(balance);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
