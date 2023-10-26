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
using System.Linq;
using System.Security.AccessControl;
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

        public ValueTask<Transfer> MakeAndAddTransferAsync(Account account, 
            string sourceAccountNumber, string receiverAccountNumber, decimal amount) =>
        TryCatch(async () =>
        {
            var transfer = CreateTransfer(account, sourceAccountNumber, receiverAccountNumber, amount);

            ValidateTransferOnAdd(transfer);

            return await this.storageBroker.InsertTransferAsync(transfer);
        });

        public ValueTask<Transfer> RetrieveTransferByIdAsync(Guid transferId) =>
        TryCatch(async () =>
        {
            ValidateTransferId(transferId);

            Transfer maybeTransfer = await this.storageBroker.SelectTransferByIdAsync(transferId);

            ValidateStorageTransfer(maybeTransfer, transferId);

            return maybeTransfer;
        });

        public IQueryable<Transfer> RetrieveAllTransfers() =>
            TryCatch(() => this.storageBroker.SelectAllTransfers());

        public ValueTask<Transfer> ModifyTransferAsync(Transfer transfer) =>
        TryCatch(async () =>
        {
            ValidateTransferOnModify(transfer);

            Transfer maybeTransfer =
                await this.storageBroker.SelectTransferByIdAsync(transfer.TransferId);

            ValidateAndAgainstStorageTransferOnModify(transfer, maybeTransfer);

            return await this.storageBroker.UpdateTransferAsync(transfer);
        });

        public ValueTask<Transfer> RemoveTransferByIdAsync(Guid transferId) =>
         TryCatch(async () =>
         {
             ValidateTransferId(transferId);

             Transfer maybeTransfer =
                 await this.storageBroker.SelectTransferByIdAsync(transferId);

             ValidateStorageTransfer(maybeTransfer, transferId);

             return await this.storageBroker.DeleteTransferAsync(maybeTransfer);
         });

        private static Transfer CreateTransfer(Account account, string sourceAccountNumber,
            string receiverAccountNumber, decimal amount)
        {
            return new Transfer
            {
                TransferId = Guid.NewGuid(),
                Account = account,
                Amount = amount,
                SourceAccountNumber = sourceAccountNumber,
                ReceiverAccountNumber = receiverAccountNumber,
            };
        }
    }
}
