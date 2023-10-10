//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using EasyPay.Api.Models.Clients;
using System.Threading.Tasks;

namespace EasyPay.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<Client> InsertClientAsync(Client salary);
    }
}
