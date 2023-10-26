//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using EasyPay.Api.Models.Accounts;
using EasyPay.Api.Models.Transfers;
using System.Linq;
using System;
using System.Threading.Tasks;

namespace EasyPay.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<Account> SelectAccountByAccountNumberAsync(string accountNumber);
        ValueTask SaveChangesTransferAsync(Account account);
        ValueTask<Transfer> InsertTransferAsync(Transfer transfer);
        IQueryable<Transfer> SelectAllTransfers();
        ValueTask<Transfer> SelectTransferByIdAsync(Guid TransferId);
        ValueTask<Transfer> UpdateTransferAsync(Transfer transfer);
        ValueTask<Transfer> DeleteTransferAsync(Transfer Transfer);
    }
}
