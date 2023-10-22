//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using EasyPay.Api.Brokers.DateTimes;
using EasyPay.Api.Brokers.Loggings;
using EasyPay.Api.Brokers.Storages;
using EasyPay.Api.Models.Accounts;
using EasyPay.Api.Models.Transfers;
using System;
using System.Threading.Tasks;

namespace EasyPay.Api.Services.Foundations.Transfers
{
    public partial class TransferService
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

        public async ValueTask<decimal> MakeTransferAsync(
            string sourceAccountNumber, string receiverAccountNumber, decimal amount)
        {
            var sourceAccount = await this.storageBroker
                .SelectAccountByAccountNumberAsync(sourceAccountNumber);
            var recieverAccount = await this.storageBroker
                .SelectAccountByAccountNumberAsync(receiverAccountNumber);

            ValidateAccountNotFoundForTransfer(sourceAccount, recieverAccount);

            ValidateAccountInsufficientFunds(amount, sourceAccount);

            sourceAccount.Balance -= amount;
            await this.storageBroker.SaveChangesTransferAsync(sourceAccount);
            recieverAccount.Balance += amount;
            await this.storageBroker.SaveChangesTransferAsync(recieverAccount);

            await AddTransferAsync(receiverAccountNumber, sourceAccountNumber, amount);

            return sourceAccount.Balance;
        }

        public async ValueTask<decimal> DepositAsync(string accountNumber, decimal amount)
        {
            var account = await this.storageBroker
                .SelectAccountByAccountNumberAsync(accountNumber);

            ValidateAccountNotFoundForTransfer(account);

            account.Balance += amount;

            await this.storageBroker.SaveChangesTransferAsync(account);



            return account.Balance;
        }

        public async ValueTask<decimal> CheckBalanceAsync(string accountNumber)
        {
            var account = await this.storageBroker
                .SelectAccountByAccountNumberAsync(accountNumber);

            ValidateAccountNotFoundForTransfer(account);

            return account.Balance;
        }

        public async ValueTask<Transfer> AddTransferAsync(string receiverAccountNumber, string sourceAccountNumber, decimal amount)
        {
            Transfer transfer = new Transfer
            {
                TransferId = Guid.NewGuid(),
                Amount = amount,
                ReceiverAccountNumber = receiverAccountNumber,
                SourceAccountNumber = sourceAccountNumber,
            };

            return await this.storageBroker.InsertTransferAsync(transfer);
        }
    }
}
