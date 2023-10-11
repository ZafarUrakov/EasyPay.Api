//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using EasyPay.Api.Brokers.DateTimes;
using EasyPay.Api.Brokers.Loggings;
using EasyPay.Api.Brokers.Storages;
using EasyPay.Api.Models.Clients;
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

        public async ValueTask<Client> AddClientAsync(Client client)
        {
            ValidateClientOnAdd(client);

            return await this.storageBroker.InsertClientAsync(client);
        }
    }
}
