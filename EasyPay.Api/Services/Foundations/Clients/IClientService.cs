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
        /// <exception cref="Models.Clients.Exceptions.ClientDependencyException"></exception>
        /// <exception cref="Models.Clients.Exceptions.ClientServiceException"></exception>     
        IQueryable<Client> RetrieveAllClients();
        /// <exception cref="Models.Clients.Exceptions.ClientDependencyException"></exception>
        /// <exception cref="Models.Clients.Exceptions.ClientServiceException"></exception>   
        ValueTask<Client> RetrieveClientByIdAsync(Guid clientId);
        /// <exception cref="Models.Clients.Exceptions.ClientDependencyValidationException"></exception>
        /// <exception cref="Models.Clients.Exceptions.ClientDependencyException"></exception>
        /// <exception cref="Models.Clients.Exceptions.ClientServiceException"></exception>
        ValueTask<Client> RemoveClientByIdAsync(Guid locationId);
        /// <exception cref="Models.Clients.Exceptions.ClientValidationException"></exception>
        /// <exception cref="Models.Clients.Exceptions.ClientDependencyValidationException"></exception>
        /// <exception cref="Models.Clients.Exceptions.ClientDependencyException"></exception>
        /// <exception cref="Models.Clients.Exceptions.ClientServiceException"></exception>
        ValueTask<Client> ModifyClientAsync(Client client);
    }
}
