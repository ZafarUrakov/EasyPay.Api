//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using EasyPay.Api.Models.Images;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EasyPay.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<ImageMetadata> InsertImageMetadataAsync(ImageMetadata imageMetadata);
        IQueryable<ImageMetadata> SelectAllImageMetadatas();
        ValueTask<ImageMetadata> SelectImageMetadataByIdAsync(Guid imageMetadataId);
        ValueTask<ImageMetadata> UpdateImageMetadataAsync(ImageMetadata imageMetadata);
        ValueTask<ImageMetadata> DeleteImageMetadataAsync(ImageMetadata imageMetadata);
    }
}
