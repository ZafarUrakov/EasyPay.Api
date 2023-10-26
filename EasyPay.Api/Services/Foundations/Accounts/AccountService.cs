//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using EasyPay.Api.Brokers.DateTimes;
using EasyPay.Api.Brokers.Loggings;
using EasyPay.Api.Brokers.Storages;
using EasyPay.Api.Models.Accounts;
using EasyPay.Api.Models.Accounts.Exceptions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EasyPay.Api.Services.Foundations.Accounts
{
    public partial class AccountService : IAccountService
    {
        private readonly IStorageBroker storageBroker;
        private readonly ILoggingBroker loggingBroker;
        private readonly IDateTimeBroker dateTimeBroker;

        public AccountService(
            IStorageBroker storageBroker,
            ILoggingBroker loggingBroker,
            IDateTimeBroker dateTimeBroker)
        {
            this.storageBroker = storageBroker;
            this.loggingBroker = loggingBroker;
            this.dateTimeBroker = dateTimeBroker;
        }

        public ValueTask<Account> AddAccountAsync(Account account) =>
        TryCatch(async () =>
        {
            ValidateAccountOnAdd(account);

            return await this.storageBroker.InsertAccountAsync(account);
        });


        public ValueTask<Account> RetrieveAccountByIdAsync(Guid accountId) =>
        TryCatch(async () =>
        {
            ValidateAccountId(accountId);

            var maybeAccount = 
                await this.storageBroker.SelectAccountByIdAsync(accountId);

            ValidateStorageAccount(maybeAccount, accountId);

            return maybeAccount;
        });

        public IQueryable<Account> RetrieveAllAccounts() =>
             TryCatch(() => this.storageBroker.SelectAllAccounts());

        public ValueTask<Account> RetrieveAccountByAccountNumberAsync(string accountNumber) =>
        TryCatch(async () =>
        {
            var maybeAccount = 
                await this.storageBroker.SelectAccountByAccountNumberAsync(accountNumber);

            ValidateAccountNotFoundByAccountNumber(maybeAccount, accountNumber);

            return maybeAccount;
        });

        public async ValueTask<Account> RetrieveAccountByLogingAndAccountNumberAsync(
            string login, string accountNumber)
        {
            var maybeAccount = await this.storageBroker
                .SelectAccountByLoginAndAccountNumber(login, accountNumber);

            ValidateAccountNotFoundWithLoginOrAccountNumber(login, accountNumber, maybeAccount);

            return maybeAccount;
        }


        public ValueTask<Account> ModifyAccountAsync(Account account) =>
        TryCatch(async () =>
        {
            ValidateAccountOnModify(account);

            var maybeAccount =
                await this.storageBroker.SelectAccountByIdAsync(account.AccountId);

            ValidateAgainstStorageAccountOnModify(inputAccount: account, storageAccount: maybeAccount);

            return await this.storageBroker.UpdateAccountAsync(account);
        });

        public ValueTask<Account> RemoveAccountByIdAsync(Guid accountId) =>
        TryCatch(async () =>
        {
            ValidateAccountId(accountId);

            var maybeAccount =
                await this.storageBroker.SelectAccountByIdAsync(accountId);

            ValidateStorageAccount(maybeAccount, accountId);

            return await this.storageBroker.DeleteAccountAsync(maybeAccount);
        });
    }
}
