//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using EasyPay.Api.Brokers.DateTimes;
using EasyPay.Api.Brokers.Loggings;
using EasyPay.Api.Models.Images;
using EasyPay.Api.Services.Foundations.ImagaMetadatas;
using System;
using System.IO;
using System.Threading.Tasks;

namespace EasyPay.Api.Services.Processings.ImageMetadatas
{
    public class ImagaMetadataProcessingService : IImagaMetadataProcessingService
    {
        private readonly IImageMetadataService imageMetadataService;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public ImagaMetadataProcessingService(
            IImageMetadataService imageMetadataService,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.imageMetadataService = imageMetadataService;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }
        
        public async ValueTask<ImageMetadata> StoreImageAsync(MemoryStream memoryStream, Guid clientId)
        {
            ImageMetadata imageMetadata = CreateImageMetada(memoryStream, clientId);

            return await this.imageMetadataService.AddImageMetadataAsync(imageMetadata);
        }

        private ImageMetadata CreateImageMetada(MemoryStream memoryStream, Guid clientId)
        {
            Guid id = Guid.NewGuid();

            return new ImageMetadata
            {
                Id = id,
                Name = id.ToString(),
                Size = memoryStream.Length,
                ClientId = clientId
            };
        }
    }
}
