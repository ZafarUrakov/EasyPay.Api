using EasyPay.Api.Models.Accounts.Exceptions;
using EasyPay.Api.Models.Clients;
using EasyPay.Api.Models.Transfers;
using EasyPay.Api.Models.Transfers.Exceptions;
using EasyPay.Api.Services.Foundations.Transfers;
using EasyPay.Api.Services.Processings.Transfers;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EasyPay.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransferController : RESTFulController
    {

        private readonly ITransferProcessingService transferProcessingService;

        public TransferController(ITransferProcessingService transferProcessingService)
        {
            this.transferProcessingService = transferProcessingService;
        }

        [HttpPost("Transfer")]
        public async Task<ActionResult> Transfer(string sourceAccountNumber, string receiverAccountNumber, decimal amount)
        {
            try
            {
                var updatedBalance = await this.transferProcessingService
                    .MakeAndInsertTransferAsync(sourceAccountNumber, receiverAccountNumber, amount);

                return Ok("Your balance: " + updatedBalance);
            }
            catch (AccountValidationException accountValidationException)
            {
                return BadRequest(accountValidationException.InnerException);
            }
            catch (AccountDependencyValidationException dependencyValidationException)
            {
                return Conflict(dependencyValidationException.InnerException);
            }
            catch (AccountDependencyException dependencyException)
            {
                return InternalServerError(dependencyException.InnerException);
            }
            catch (AccountServiceException serviceException)
            {
                return InternalServerError(serviceException.InnerException);
            }
            catch (TransferValidationException transferValidationException)
            {
                return BadRequest(transferValidationException.InnerException);
            }
            catch (TransferDependencyException dependencyException)
            {
                return InternalServerError(dependencyException.InnerException);
            }
            catch (TransferServiceException serviceException)
            {
                return InternalServerError(serviceException.InnerException);
            }
        }

        [HttpPost("Deposit")]
        public async ValueTask<IActionResult> Deposit(string accountNumber, decimal amount)
        {
            try
            {
                var updatedBalance = await transferProcessingService.DepositAsync(accountNumber, amount);

                return Ok("Your balance: " + updatedBalance);
            }
            catch (AccountValidationException accountValidationException)
            {
                return BadRequest(accountValidationException.InnerException);
            }
            catch (AccountDependencyValidationException dependencyValidationException)
            {
                return Conflict(dependencyValidationException.InnerException);
            }
            catch (AccountDependencyException dependencyException)
            {
                return InternalServerError(dependencyException.InnerException);
            }
            catch (AccountServiceException serviceException)
            {
                return InternalServerError(serviceException.InnerException);
            }
            catch (TransferValidationException transferValidationException)
            {
                return BadRequest(transferValidationException.InnerException);
            }
        }

        [HttpGet("Balance")]
        public async Task<IActionResult> GetBalance(string accountNumber)
        {
            try
            {
                var balance = await transferProcessingService.CheckBalanceAsync(accountNumber);

                return Ok("Your balance: " + balance);
            }
            catch (AccountValidationException accountValidationException)
            {
                return BadRequest(accountValidationException.InnerException);
            }
            catch (AccountDependencyValidationException dependencyValidationException)
            {
                return Conflict(dependencyValidationException.InnerException);
            }
            catch (AccountDependencyException dependencyException)
            {
                return InternalServerError(dependencyException.InnerException);
            }
            catch (AccountServiceException serviceException)
            {
                return InternalServerError(serviceException.InnerException);
            }
        }

        [HttpGet("ById")]
        public async ValueTask<ActionResult<Transfer>> GetTransferByIdAsync(Guid transferId)
        {
            var transfer = await this.transferProcessingService.RetrieveTransferByIdAsync(transferId);

            return Ok(transfer);
        }

        [HttpGet]
        public ActionResult<IQueryable<Transfer>> GetAllTransfers()
        {
            IQueryable<Transfer> transfers = this.transferProcessingService.RetrieveAllTransfers();

            return Ok(transfers);
        }

        [HttpPut]
        public async ValueTask<ActionResult<Transfer>> PutTransferAsync(Transfer transfer)
        {
            var modifyTransfer = await this.transferProcessingService.ModifyTransferAsync(transfer);

            return Ok(modifyTransfer);
        }

        [HttpDelete]
        public async ValueTask<ActionResult<Transfer>> DeleteTransferAsync(Guid tranferId)
        {
            var deleteTransfer = await this.transferProcessingService.RemoveTransferByIdAsync(tranferId);

            return Ok(deleteTransfer);
        }
    }
}
