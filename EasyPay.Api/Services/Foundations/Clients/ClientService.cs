//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using EasyPay.Api.Brokers.DateTimes;
using EasyPay.Api.Brokers.Loggings;
using EasyPay.Api.Brokers.Storages;
using EasyPay.Api.Models.Clients;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EasyPay.Api.Services.Foundations.Clients
{
    public partial class ClientService : IClientService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public ClientService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<Client> AddClientAsync(Client client) =>
        TryCatch(async () =>
        {
            ValidateClientOnAdd(client);

            return await this.storageBroker.InsertClientAsync(client);
        });

        public IQueryable<Client> RetrieveAllClients() =>
            TryCatch(() => this.storageBroker.SelectAllClients());

        public ValueTask<Client> RetrieveClientByIdAsync(Guid clientId) =>
        TryCatch(async () =>
        {
            ValidateClientId(clientId);

            Client maybeclient =  await this.storageBroker.SelectClientByIdAsync(clientId);

            ValidateStorageClient(maybeclient, clientId);

            return maybeclient;
        });

        public ValueTask<Client> RemoveClientByIdAsync(Guid clientId) =>
        TryCatch(async () =>
        {
            ValidateClientId(clientId);

            Client maybeclient =
                await this.storageBroker.SelectClientByIdAsync(clientId);

            ValidateStorageClient(maybeclient, clientId);

            return await this.storageBroker.DeleteClientAsync(maybeclient);
        });

        public ValueTask<Client> ModifyClientAsync(Client client) =>
        TryCatch(async () =>
        {
            ValidateClientOnAdd(client);

            Client maybeClient = await this.storageBroker.SelectClientByIdAsync(client.ClientId);

            ValidateStorageClient(maybeClient, client.ClientId);

            return await this.storageBroker.UpdateClientAsync(client);
        });
        
    }
}
