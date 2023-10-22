//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using EasyPay.Api.Models.Accounts;
using System.Threading.Tasks;

namespace EasyPay.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<Account> SelectAccountByAccountNumberAsync(string accountNumber);
        ValueTask SaveChangesTransferAsync(Account account);
    }
}
