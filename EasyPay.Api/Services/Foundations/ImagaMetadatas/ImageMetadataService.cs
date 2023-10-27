//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using EasyPay.Api.Brokers.DateTimes;
using EasyPay.Api.Brokers.Loggings;
using EasyPay.Api.Brokers.Storages;
using EasyPay.Api.Models.Images;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EasyPay.Api.Services.Foundations.ImagaMetadatas
{
    public class ImageMetadataService : IImageMetadataService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public ImageMetadataService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }
        public async ValueTask<ImageMetadata> AddImageMetadataAsync(ImageMetadata imageMetadata)
        {
            return await this.storageBroker.InsertImageMetadataAsync(imageMetadata);
        }


        public IQueryable<ImageMetadata> RetrieveAllImageMetadatas()
        {
            return this.storageBroker.SelectAllImageMetadatas();
        }

        public async ValueTask<ImageMetadata> RetrieveImageMetadataByIdAsync(Guid imageMetadataId)
        {
            return await this.storageBroker.SelectImageMetadataByIdAsync(imageMetadataId);
        }

        public async ValueTask<ImageMetadata> ModifydImageMetadataAsync(ImageMetadata imageMetadata)
        {
            return await this.storageBroker.UpdateImageMetadataAsync(imageMetadata);
        }

        public async ValueTask<ImageMetadata> RemovedImageMetadataByIdAsync(Guid locationId)
        {
            var maybeImageMetadata =
                await this.storageBroker.SelectImageMetadataByIdAsync(locationId);

            return await this.storageBroker.DeleteImageMetadataAsync(maybeImageMetadata);
        }
    }
}
