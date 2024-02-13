//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using EasyPay.Api.Models.Images;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EasyPay.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<ImageMetadata> ImageMetadatas { get; set; }

        public async ValueTask<ImageMetadata> InsertImageMetadataAsync(ImageMetadata imageMetadata) =>
            await InsertAsync(imageMetadata);

        public IQueryable<ImageMetadata> SelectAllImageMetadatas() =>
            SelectAll<ImageMetadata>();

        public async ValueTask<ImageMetadata> SelectImageMetadataByIdAsync(Guid imageMetadataId) =>
            await SelectAsync<ImageMetadata>(imageMetadataId);

        public async ValueTask<ImageMetadata> UpdateImageMetadataAsync(ImageMetadata imageMetadata) =>
            await UpdateAsync(imageMetadata);

        public async ValueTask<ImageMetadata> DeleteImageMetadataAsync(ImageMetadata imageMetadata) =>
            await DeleteAsync(imageMetadata);
    }
}
