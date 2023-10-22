//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using System;
using System.Threading.Tasks;
using EasyPay.Api.Brokers.Storages;
using EasyPay.Api.Models.Accounts;
using EasyPay.Api.Services.Foundations.Accounts;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;

namespace EasyPay.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : RESTFulController
    {
        private readonly IAccountService accountService;
        private readonly IStorageBroker storageBroker;

        public AccountController(IAccountService accountService, IStorageBroker storageBroker)
        {
            this.accountService = accountService;
            this.storageBroker = storageBroker;
        }

        [HttpPost]
        public async ValueTask<ActionResult<Account>> PostAccount(Account account)
        {
            return await this.accountService.AddAccountAsync(account);
        }

        [HttpGet("account/{accountId}/balance")]
        public async ValueTask<IActionResult> GetAccountBalance(Guid accountId)
        {
            var account = await accountService.RemoveAccountByIdAsync(accountId);

            if (account == null)
            {
                return NotFound("Account not found");
            }

            decimal balance = account.Balance;
            return Ok(balance);
        }
    }
}
