//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using EasyPay.Api.Models.Accounts;
using EasyPay.Api.Models.Clients;
using EasyPay.Api.Services.Foundations.Accounts;
using EasyPay.Api.Services.Foundations.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyPay.Api.Services.Processings.Clients
{
    public class ClientProcessingService : IClientProcessingService
    {
        private readonly IClientService clientService;
        private readonly IAccountService accountService;

        public ClientProcessingService(IAccountService accountService, IClientService clientService)
        {
            this.accountService = accountService;
            this.clientService = clientService;
        }

        public async ValueTask<string> RegisterClientWithAccountAsync(Client client)
        {
            client.ClientId = Guid.NewGuid();
            client.AccountNumber = RandomNumber().ToString();
            client.Accounts = new List<Account>();

            Account account = new Account
            {
                AccountId = Guid.NewGuid(),
                Login = client.Login,
                AccountNumber = client.AccountNumber,
                ClientId = client.ClientId,
                Client = client,
            };

            await clientService.AddClientAsync(client);

            await accountService.AddAccountAsync(account);

            return client.AccountNumber;
        }

        public async ValueTask<Client> RetrieveClientByIdAsync(Guid clientId) =>
            await clientService.RetrieveClientByIdAsync(clientId);

        public IQueryable<Client> RetrieveAllClients() =>
            clientService.RetrieveAllClients();

        public async ValueTask<Client> ModifyClientAsync(Client client) =>
            await clientService.ModifyClientAsync(client);

        public async ValueTask<Client> RemoveClientByIdAsync(Guid clientId) =>
            await clientService.RemoveClientByIdAsync(clientId);

        public int RandomNumber()
        {
            Random random = new Random();

            return random.Next(10000000, 99999999);
        }
    }
}
