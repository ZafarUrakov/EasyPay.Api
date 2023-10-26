//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using System.Linq;
using System.Threading.Tasks;
using System;
using EasyPay.Api.Models.Images;

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
