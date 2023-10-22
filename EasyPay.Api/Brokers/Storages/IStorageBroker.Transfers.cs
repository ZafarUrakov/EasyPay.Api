//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using EasyPay.Api.Models.Accounts;
using EasyPay.Api.Models.Transfers;
using System.Threading.Tasks;

namespace EasyPay.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<Account> SelectAccountByAccountNumberAsync(string accountNumber);
        ValueTask SaveChangesTransferAsync(Account account);
        ValueTask<Transfer> InsertTransfer(Transfer transfer);
    }
}
