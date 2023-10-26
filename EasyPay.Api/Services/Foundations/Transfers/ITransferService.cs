//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using EasyPay.Api.Models.Accounts;
using EasyPay.Api.Models.Transfers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EasyPay.Api.Services.Foundations.Transfers
{
    public interface ITransferService
    {
        ValueTask<Transfer> MakeAndAddTransferAsync(Account account,
            string sourceAccountNumber, string receiverAccountNumber, decimal amount);
        IQueryable<Transfer> RetrieveAllTransfers(); 
        ValueTask<Transfer> RetrieveTransferByIdAsync(Guid TransferId);
        ValueTask<Transfer> ModifyTransferAsync(Transfer Transfer);
        ValueTask<Transfer> RemoveTransferByIdAsync(Guid TransferId);
    }
}
