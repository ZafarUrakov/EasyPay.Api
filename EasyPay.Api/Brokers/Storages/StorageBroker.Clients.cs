//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using EasyPay.Api.Models.Accounts;
using EasyPay.Api.Models.Clients;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EasyPay.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Client> Clients { get; set; }

        public async ValueTask<Client> InsertClientAsync(Client client) =>
            await InsertAsync(client);

        public IQueryable<Client> SelectAllClients()
        {
            var clients = SelectAll<Client>().Include(a => a.Accounts);

            return clients;
        }

        public async ValueTask<Client> SelectClientByIdAsync(Guid clientId)
        {
            var clientWithAccounts = Clients
                .Include(c => c.Accounts)
                .FirstOrDefault(c => c.ClientId == clientId);

            return await ValueTask.FromResult(clientWithAccounts);
        }


        public async ValueTask<Client> UpdateClientAsync(Client client) =>
            await UpdateAsync(client);

        public async ValueTask<Client> DeleteClientAsync(Client client) =>
            await DeleteAsync(client);
    }
}
