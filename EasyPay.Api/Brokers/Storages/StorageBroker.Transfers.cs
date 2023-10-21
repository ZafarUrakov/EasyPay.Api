//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using EasyPay.Api.Models.Transfers;
using Microsoft.EntityFrameworkCore;

namespace EasyPay.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        DbSet<Transfer> Transfers { get; set; }
    }
}
