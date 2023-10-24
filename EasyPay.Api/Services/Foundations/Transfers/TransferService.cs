//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using EasyPay.Api.Brokers.DateTimes;
using EasyPay.Api.Brokers.Loggings;
using EasyPay.Api.Brokers.Storages;
using EasyPay.Api.Models.Transfers;
using System;
using System.Threading.Tasks;

namespace EasyPay.Api.Services.Foundations.Transfers
{
    public partial class TransferService : ITransferService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public TransferService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<decimal> MakeTransferAsync(
            string sourceAccountNumber, string receiverAccountNumber, decimal amount) =>
        TryCatch(async () =>
        {
            var sourceAccount = await this.storageBroker
                .SelectAccountByAccountNumberAsync(sourceAccountNumber);

            var recieverAccount = await this.storageBroker
                .SelectAccountByAccountNumberAsync(receiverAccountNumber);

            ValidateAccountNotFoundForTransfer(sourceAccount, sourceAccountNumber,
                recieverAccount, receiverAccountNumber);

            ValidateTransferAmount(amount);

            ValidateAccountInsufficientFunds(amount, sourceAccount);

            sourceAccount.Balance -= amount;
            recieverAccount.Balance += amount;

            await this.storageBroker.SaveChangesTransferAsync(sourceAccount);
            await this.storageBroker.SaveChangesTransferAsync(recieverAccount);

            await AddTransferAsync(receiverAccountNumber, sourceAccountNumber, amount); // processing

            return sourceAccount.Balance;
        });

        public ValueTask<decimal> DepositAsync(string accountNumber, decimal amount) =>
        TryCatch(async () =>
        {
            var account = await this.storageBroker
                .SelectAccountByAccountNumberAsync(accountNumber);

            ValidateAccountNotFoundForTransfer(account, accountNumber);

            ValidateTransferAmount(amount);

            account.Balance += amount;

            await this.storageBroker.SaveChangesTransferAsync(account);

            return account.Balance;
        });

        public ValueTask<decimal> CheckBalanceAsync(string accountNumber) =>
        TryCatch(async () =>
        {
            var account = await this.storageBroker
                .SelectAccountByAccountNumberAsync(accountNumber);

            ValidateAccountNotFoundForTransfer(account, accountNumber);

            return account.Balance;
        });

        public ValueTask<Transfer> AddTransferAsync(
            string receiverAccountNumber, string sourceAccountNumber, decimal amount) =>
        TryCatch(async () =>
        {
            Transfer transfer = new Transfer
            {
                TransferId = Guid.NewGuid(),
                Amount = amount,
                ReceiverAccountNumber = receiverAccountNumber,
                SourceAccountNumber = sourceAccountNumber,
            };

            ValidateTransferOnAdd(transfer);

            return await this.storageBroker.InsertTransferAsync(transfer);
        });
    }
}
