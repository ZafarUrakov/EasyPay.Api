//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using EasyPay.Api.Models.Accounts;
using EasyPay.Api.Models.Transfers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EasyPay.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        DbSet<Transfer> Transfers { get; set; }

        public async ValueTask<Transfer> InsertTransferAsync(Transfer transfer) =>
            await InsertAsync(transfer);

        public IQueryable<Transfer> SelectAllTransfers()
        {
            var transfers = SelectAll<Transfer>().Include(a => a.Account);

            return transfers;
        }

        public async ValueTask<Transfer> SelectTransferByIdAsync(Guid transferId)
        {
            var transferWithTransfers = Transfers
            .Include(t => t.Account)
                .FirstOrDefault(t => t.TransferId == transferId);

            return await ValueTask.FromResult(transferWithTransfers);
        }

        public async ValueTask<Transfer> UpdateTransferAsync(Transfer transfer) =>
            await UpdateAsync(transfer);

        public async ValueTask<Transfer> DeleteTransferAsync(Transfer transfer) =>
             await DeleteAsync(transfer);


        public async ValueTask SaveChangesTransferAsync(Account account)
        {
            var broker = new StorageBroker(this.configuration);
            broker.Entry(account).State = EntityState.Modified;

            await broker.SaveChangesAsync();
        }
    }
}
