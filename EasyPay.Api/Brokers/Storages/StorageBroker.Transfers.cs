//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using EasyPay.Api.Models.Accounts;
using EasyPay.Api.Models.Transfers;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace EasyPay.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        DbSet<Transfer> Transfers { get; set; }

        public async ValueTask<Transfer> InsertTransferAsync(Transfer transfer)
        {
            await this.Transfers.AddAsync(transfer);
            await this.SaveChangesAsync();

            return transfer;
        }

        public async ValueTask<Account> SelectAccountByAccountNumberAsync(string accountNumber)
        {
            Account account = await this.Accounts
                .FirstOrDefaultAsync(a => a.AccountNumber == accountNumber);

            return account;
        }

        public async ValueTask SaveChangesTransferAsync(Account account)
        {
            var broker = new StorageBroker(this.configuration);
            broker.Entry(account).State = EntityState.Modified;

            await broker.SaveChangesAsync();
        }

    }
}
