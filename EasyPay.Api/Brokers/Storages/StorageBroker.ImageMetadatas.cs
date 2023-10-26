//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using EasyPay.Api.Models.Images;
using Microsoft.EntityFrameworkCore;

namespace EasyPay.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<ImageMetadata> ImageMetadatas { get; set; }
    }
}
