//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using EasyPay.Api.Models.Clients;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace EasyPay.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Client> Clients { get; set; }

        public async ValueTask<Client> InsertClientAsync(Client client) =>
            await InsertAsync(client);

        public IQueryable<Client> SelectAllClients(Client client) =>
            SelectAll(client);
    }
}
