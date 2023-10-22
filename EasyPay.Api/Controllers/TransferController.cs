using EasyPay.Api.Models.Transfers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using EasyPay.Api.Services.Foundations.Transfers;

namespace EasyPay.Api.Controllers
{
    public class TransferController : Controller
    {
        private readonly TransferService transferService;

        public TransferController(TransferService transferService)
        {
            this.transferService = transferService;
        }

        [HttpPost("transfer")]
        public async Task<IActionResult> Transfer(string sourceAccountNumber, string receiverAccountNumber, decimal amount)
        {
            try
            {
                var updatedBalance = await transferService
                    .MakeTransferAsync(sourceAccountNumber, receiverAccountNumber, amount);

                return Ok(updatedBalance);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
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
