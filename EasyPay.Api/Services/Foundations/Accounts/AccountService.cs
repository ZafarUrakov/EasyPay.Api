//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using EasyPay.Api.Brokers.DateTimes;
using EasyPay.Api.Brokers.Loggings;
using EasyPay.Api.Brokers.Storages;
using EasyPay.Api.Models.Accounts;
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

        public IQueryable<Account> RetrieveAllAccounts()
        {
            throw new NotImplementedException();
            //return this.storageBroker.SelectAllAccounts();
        }
    }
}
