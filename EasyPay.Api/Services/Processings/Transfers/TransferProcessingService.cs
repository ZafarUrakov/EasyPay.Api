//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using EasyPay.Api.Brokers.DateTimes;
using EasyPay.Api.Brokers.Loggings;
using EasyPay.Api.Brokers.Storages;
using EasyPay.Api.Models.Accounts;
using EasyPay.Api.Models.Transfers;
using EasyPay.Api.Services.Foundations.Accounts;
using EasyPay.Api.Services.Foundations.Transfers;
using EasyPay.Api.Services.Processings.Transfers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EasyPay.Api.Services.Processings
{
    public partial class TransferProcessingService : ITransferProcessingService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;
        private readonly ITransferService transferService;
        private readonly IAccountService accountService;

        public TransferProcessingService(
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker,
            ITransferService transferService,
            IAccountService accountService,
            IStorageBroker storageBroker)
        {
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
            this.transferService = transferService;
            this.accountService = accountService;
            this.storageBroker = storageBroker;
        }

        public ValueTask<decimal> MakeAndInsertTransferAsync(
            string sourceAccountNumber, string receiverAccountNumber, decimal amount) =>
        TryCatch(async () =>
        {
            var sourceAccount = await GetAccountAndValidateAsync(sourceAccountNumber);
            var recieverAccount = await GetAccountAndValidateAsync(receiverAccountNumber);

            ValidateTransferOnAdd(sourceAccount, amount);

            sourceAccount.Balance -= amount;
            recieverAccount.Balance += amount;

            await this.storageBroker.SaveChangesTransferAsync(sourceAccount);
            await this.storageBroker.SaveChangesTransferAsync(recieverAccount);

            await this.transferService.MakeAndAddTransferAsync(sourceAccount,
                sourceAccountNumber, receiverAccountNumber, amount);

            return sourceAccount.Balance;
        });

        public ValueTask<decimal> DepositAsync(string accountNumber, decimal amount) =>
        TryCatch(async () =>
        {
            var account = await this.accountService
                .RetrieveAccountByAccountNumberAsync(accountNumber);

            ValidateTransferAmount(amount);

            account.Balance += amount;

            await this.storageBroker.SaveChangesTransferAsync(account);

            return account.Balance;
        });

        public ValueTask<decimal> CheckBalanceAsync(string accountNumber) =>
        TryCatch(async () =>
        {
            var account = await this.accountService
                .RetrieveAccountByAccountNumberAsync(accountNumber);

            return account.Balance;
        });

        public async ValueTask<Transfer> RetrieveTransferByIdAsync(Guid transferId) =>
            await this.transferService.RetrieveTransferByIdAsync(transferId);

        public IQueryable<Transfer> RetrieveAllTransfers() =>
            this.transferService.RetrieveAllTransfers();

        public async ValueTask<Transfer> ModifyTransferAsync(Transfer transfer) =>
            await this.transferService.ModifyTransferAsync(transfer);

        public async ValueTask<Transfer> RemoveTransferByIdAsync(Guid transferId) =>
            await this.transferService.RemoveTransferByIdAsync(transferId);

        private async ValueTask<Account> GetAccountAndValidateAsync(string accountNumber)
        {
            var account = await this.accountService.RetrieveAccountByAccountNumberAsync(accountNumber);

            return account;
        }
    }
}
