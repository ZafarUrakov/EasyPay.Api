//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using EasyPay.Api.Models.Accounts;
using EasyPay.Api.Services.Foundations.Accounts;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EasyPay.Api.Services.Processings.Accounts
{
    public class AccountProcessingService : IAccountProcessingService
    {
        private readonly IAccountService accountService;

        public AccountProcessingService(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        public async ValueTask<Account> RegisterAndSaveAccountAsync(Account account)
        {
            var maybeAccount = await accountService
                .RetrieveAccountByLogingAndAccountNumberAsync(account.Login, account.AccountNumber);

            Account fullAccount = new Account
            {
                AccountId = maybeAccount.AccountId,
                Password = account.Password,
                Login = account.Login,
                AccountNumber = account.AccountNumber,
                Client = maybeAccount.Client,
                ClientId = maybeAccount.ClientId
            };

            await accountService.ModifyAccountAsync(fullAccount);

            return fullAccount;
        }

        public async ValueTask<Account> RetrieveAccountByIdAsync(Guid accountId) =>
            await accountService.RetrieveAccountByIdAsync(accountId);

        public IQueryable<Account> RetrieveAllAccounts() =>
            accountService.RetrieveAllAccounts();

        public async ValueTask<Account> ModifyAccountAsync(Account account) =>
            await accountService.ModifyAccountAsync(account);

        public async ValueTask<Account> RemoveAccountByIdAsync(Guid accountId) =>
            await accountService.RemoveAccountByIdAsync(accountId);
    }
}
