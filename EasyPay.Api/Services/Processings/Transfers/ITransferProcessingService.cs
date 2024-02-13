//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using EasyPay.Api.Models.Transfers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EasyPay.Api.Services.Processings.Transfers
{
    public interface ITransferProcessingService
    {
        ValueTask<decimal> MakeAndInsertTransferAsync(
            string receiverAccountNumber, string sourceAccountNumber, decimal amount);
        ValueTask<decimal> DepositAsync(string accountNumber, decimal amount);
        ValueTask<decimal> CheckBalanceAsync(string accountNumber);
        IQueryable<Transfer> RetrieveAllTransfers();
        ValueTask<Transfer> RetrieveTransferByIdAsync(Guid TransferId);
        ValueTask<Transfer> ModifyTransferAsync(Transfer Transfer);
        ValueTask<Transfer> RemoveTransferByIdAsync(Guid TransferId);
    }
}
