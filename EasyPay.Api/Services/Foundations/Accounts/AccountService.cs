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
using System.Data;
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

        public async ValueTask<Account> ModifyAccountAsync(Account account)
        {
            try
            {
                //Account maybeAccount =
                //    await this.storageBroker.SelectAccountByIdAsync(account.AccountId);

                //if (account is null)
                //{
                //    throw new NullAccountException();
                //}

                ValidateAccountOnModify(account);

                return await this.storageBroker.UpdateAccountAsync(account);

            }
            catch (NullAccountException nullAccountException)
            {
                var accountValidationException =
                    new AccountValidationException(nullAccountException);

                this.loggingBroker.LogError(accountValidationException);
                throw accountValidationException;
            }
            catch(InvalidAccountException invalidAccountException)
            {
                //AccountValidationException accountValidationException = new AccountValidationException(invalidAccountException);

                //this.loggingBroker.LogError(accountValidationException);
                //throw accountValidationException;
                throw invalidAccountException;
            }

        }

        public ValueTask<Account> RemoveAccountByIdAsync(Guid accountId) =>
        TryCatch(async () =>
        {
            ValidateAccountId(accountId);

            Account maybeAccount =
                await this.storageBroker.SelectAccountByIdAsync(accountId);

            ValidateStorageAccount(maybeAccount, accountId);

            return await this.storageBroker.DeleteAccountAsync(maybeAccount);
        });

        public ValueTask<Account> RetrieveAccountByIdAsync(Guid accountId) =>
        TryCatch(async () =>
        {
            ValidateAccountId(accountId);

            Account maybeAccount = await this.storageBroker.SelectAccountByIdAsync(accountId);

            ValidateStorageAccount(maybeAccount, accountId);

            return maybeAccount;
        });

        public IQueryable<Account> RetrieveAllAccounts() =>
             TryCatch(() => this.storageBroker.SelectAllAccounts());
    }
}
