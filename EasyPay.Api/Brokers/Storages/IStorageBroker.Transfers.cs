//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

namespace EasyPay.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        decimal GetBalance();
    }
}
