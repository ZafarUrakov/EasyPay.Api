//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using EasyPay.Api.Models.Accounts;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace EasyPay.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Account> Accounts { get; set; }

        public async ValueTask<Account> InsertAccountAsync(Account account) =>
            await InsertAsync(account);
    }
}
