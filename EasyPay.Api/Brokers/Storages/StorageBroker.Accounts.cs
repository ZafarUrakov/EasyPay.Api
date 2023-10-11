//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using EasyPay.Api.Models.Accounts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EasyPay.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Account> Accounts { get; set; }

        public async ValueTask<Account> InsertAccountAsync(Account account) =>
            await InsertAsync(account);

        public IQueryable<Account> SelectAllAccounts(Account account) =>
            SelectAll(account);

        public async ValueTask<Account> SelectAccountByIdAsync(Guid accountId) =>
            await SelectAsync<Account>(accountId);

        public async ValueTask<Account> UpdateAccountAsync(Account account) =>
            await UpdateAsync(account);

        public async ValueTask<Account> DeleteAccountAsync(Account account) =>
             await DeleteAsync(account);
    }
}
