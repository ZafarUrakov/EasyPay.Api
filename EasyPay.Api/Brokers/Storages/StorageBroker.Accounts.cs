//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using EasyPay.Api.Models.Accounts;
using Microsoft.EntityFrameworkCore;
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
    }
}
