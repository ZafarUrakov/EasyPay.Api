//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using EasyPay.Api.Models.Clients;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EasyPay.Api.Services.Foundations.Clients
{
    public interface IClientService
    {
        /// <exception cref="Models.Clients.Exceptions.ClientValidationException"></exception>
        /// <exception cref="Models.Clients.Exceptions.ClientDependencyValidationException"></exception>
        /// <exception cref="Models.Clients.Exceptions.ClientDependencyException"></exception>
        /// <exception cref="Models.Clients.Exceptions.ClientServiceException"></exception>
        ValueTask<Client> AddClientAsync(Client client);
        IQueryable<Client> RetrieveAllClients();
        ValueTask<Client> RetrieveClientByIdAsync(Guid clientId);
        ValueTask<Client> RemoveClientByIdAsync(Guid locationId);
    }
}
