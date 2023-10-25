//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using EasyPay.Api.Models.Accounts;
using EasyPay.Api.Services.Foundations.Accounts;
using System.Threading.Tasks;

namespace EasyPay.Api.Services.Processings
{
    public class AccountProcessingService
    {
        private readonly IAccountService accountService;

        public AccountProcessingService(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        public async ValueTask<Account> RegisterAndSaveAccountAsync(Account account)
        {
            var maybeAccount = await this.accountService
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

            await this.accountService.ModifyAccountAsync(fullAccount);

            return fullAccount;
        }
    }
}
