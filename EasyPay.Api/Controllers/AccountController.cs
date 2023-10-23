//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using EasyPay.Api.Brokers.Storages;
using EasyPay.Api.Models.Accounts;
using EasyPay.Api.Models.Accounts.Exceptions;
using EasyPay.Api.Services.Foundations.Accounts;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EasyPay.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : RESTFulController
    {
        private readonly IAccountService accountService;

        public AccountController(IAccountService accountService, IStorageBroker storageBroker)
        {
            this.accountService = accountService;
        }

        [HttpPost]
        public async ValueTask<ActionResult<Account>> PostAccountAsync(Account account)
        {
            try
            {
                Account persistedAccount =
                await this.accountService.AddAccountAsync(account);

                return Created(persistedAccount);
            }
            catch (AccountValidationException accountValidationException)
            {
                return BadRequest(accountValidationException.InnerException);
            }
            catch (AccountDependencyValidationException accountDependencyValidationException)
                when (accountDependencyValidationException.InnerException is AlreadyExistsAccountException)
            {
                return Conflict(accountDependencyValidationException.InnerException);
            }
            catch (AccountDependencyValidationException accountDependencyValidationException)
            {
                return BadRequest(accountDependencyValidationException.InnerException);
            }
            catch (AccountDependencyException accountDependencyException)
            {
                return InternalServerError(accountDependencyException.InnerException);
            }
            catch (AccountServiceException accountServiceException)
            {
                return InternalServerError(accountServiceException.InnerException);
            }

        }

        [HttpGet("ById")]
        public async ValueTask<ActionResult<Account>> GetAccountByIdAsync(Guid accountId)
        {
            try
            {
                Account getAccount =
                    await this.accountService.RetrieveAccountByIdAsync(accountId);

                return Created(getAccount);
            }
            catch (AccountDependencyException accountDependencyException)
            {
                return InternalServerError(accountDependencyException.InnerException);
            }
            catch (AccountValidationException accountValidationException)
                 when (accountValidationException.InnerException is NotFoundAccountException)
            {
                return BadRequest(accountValidationException.InnerException);
            }
            catch (AccountValidationException accountValidationException)
            {
                return BadRequest(accountValidationException.InnerException);
            }
            catch (AccountServiceException accountServiceException)
            {
                return InternalServerError(accountServiceException.InnerException);
            }
        }

        [HttpGet("All")]
        public ActionResult<IQueryable<Account>> GetAllAccount()
        {
            try
            {
                IQueryable<Account> accounts
                    = this.accountService.RetrieveAllAccounts();

                return Created(accounts);
            }
            catch (AccountDependencyException accountDependencyException)
            {
                return InternalServerError(accountDependencyException.InnerException);
            }
            catch (AccountServiceException accountServiceException)
            {
                return InternalServerError(accountServiceException.InnerException);
            }
        }

        [HttpPut]
        public async ValueTask<ActionResult<Account>> PutAccountAsync(Account account)
        {
            try
            {
                Account modifiedAccount =
                await this.accountService.ModifyAccountAsync(account);

                return Created(modifiedAccount);
            }
            catch (AccountValidationException accountValidationException)
                when (accountValidationException.InnerException is NotFoundAccountException)
            {
                return BadRequest(accountValidationException.InnerException);
            }
            catch (AccountValidationException accountValidationException)
            {
                return BadRequest(accountValidationException.InnerException);
            }
            catch (AccountDependencyValidationException accountDependencyValidationException)
            {
                return Conflict(accountDependencyValidationException.InnerException);
            }
            catch (AccountDependencyException accountDependencyException)
            {
                return InternalServerError(accountDependencyException.InnerException);
            }
            catch (AccountServiceException accountServiceException)
            {
                return InternalServerError(accountServiceException.InnerException);
            }
        }

        [HttpDelete]
        public async ValueTask<ActionResult<Account>> DeleteAccountAsync(Guid accountId)
        {
            try
            {
                Account deletedAccount =
                    await this.accountService.RemoveAccountByIdAsync(accountId);

                return Created(deletedAccount);
            }
            catch (AccountDependencyValidationException accountDependencyValidationException)
                when (accountDependencyValidationException.InnerException is LockedAccountException)
            {
                return Locked(accountDependencyValidationException.InnerException);
            }
            catch (AccountDependencyValidationException accountDependencyValidationException)
            {
                return BadRequest(accountDependencyValidationException.InnerException);
            }
            catch (AccountValidationException accountValidationException)
                when (accountValidationException.InnerException is NotFoundAccountException)
            {
                return BadRequest(accountValidationException.InnerException);
            }
            catch (AccountValidationException accountValidationException)
            {
                return BadRequest(accountValidationException.InnerException);
            }
            catch (AccountDependencyException accountDependencyException)
            {
                return InternalServerError(accountDependencyException.InnerException);
            }
            catch (AccountServiceException accountServiceException)
            {
                return InternalServerError(accountServiceException.InnerException);
            }

        }
    }
}
