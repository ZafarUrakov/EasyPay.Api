//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using EasyPay.Api.Models.Clients;
using System.Linq;
using System.Threading.Tasks;
using System;
using EasyPay.Api.Models.Images;

namespace EasyPay.Api.Services.Foundations.ImagaMetadatas
{
    public interface IImageMetadataService
    {
        ValueTask<ImageMetadata> AddImageMetadataAsync(ImageMetadata imageMetadata);
        IQueryable<ImageMetadata> RetrieveAllImageMetadatas();
        ValueTask<ImageMetadata> RetrieveImageMetadataByIdAsync(Guid imageMetadataId);
        ValueTask<ImageMetadata> RemovedImageMetadataByIdAsync(Guid locationId);
        ValueTask<ImageMetadata> ModifydImageMetadataAsync(ImageMetadata imageMetadata);
    }
}