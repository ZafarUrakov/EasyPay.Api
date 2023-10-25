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

        public IQueryable<Account> SelectAllAccounts()
        {
            var accounts = SelectAll<Account>().Include(a => a.Client);

            return accounts;
        }

        public async ValueTask<Account> SelectAccountByIdAsync(Guid accountId)
        {
            var clientWithAccounts = Accounts
            .Include(a => a.Client)
                .FirstOrDefault(a => a.AccountId == accountId);

            return await ValueTask.FromResult(clientWithAccounts);
        }

        public async ValueTask<Account> SelectAccountByLoginAndAccountNumber(string login, string accountNumber) =>
            await Accounts.Where(a => a.Login == login && a.AccountNumber == accountNumber)
                .FirstOrDefaultAsync();

        public async ValueTask<Account> SelectAccountByAccountNumberAsync(string accountNumber)
        {
            Account account = await this.Accounts
                .FirstOrDefaultAsync(a => a.AccountNumber == accountNumber);

            return account;
        }

        public async ValueTask<Account> UpdateAccountAsync(Account account) =>
            await UpdateAsync(account);

        public async ValueTask<Account> DeleteAccountAsync(Account account) =>
             await DeleteAsync(account);
    }
}
