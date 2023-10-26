//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using EasyPay.Api.Models.Accounts;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EasyPay.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<Account> InsertAccountAsync(Account account);
        IQueryable<Account> SelectAllAccounts();
        ValueTask<Account> SelectAccountByIdAsync(Guid accountId);
        ValueTask<Account> UpdateAccountAsync(Account account);
        ValueTask<Account> DeleteAccountAsync(Account account);
        ValueTask<Account> SelectAccountByLoginAndAccountNumber(string login, string accountNumber);
    }
}
