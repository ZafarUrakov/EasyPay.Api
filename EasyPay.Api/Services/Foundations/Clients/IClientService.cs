//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using EasyPay.Api.Models.Clients;
using System.Threading.Tasks;

namespace EasyPay.Api.Services.Foundations.Clients
{
    public interface IClientService
    {
        ValueTask<Client> AddClientAsync(Client client);
    }
}
