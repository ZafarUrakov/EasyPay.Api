//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using EasyPay.Api.Models.Images;
using System.IO;
using System.Threading.Tasks;
using System;

namespace EasyPay.Api.Services.Processings.ImageMetadatas
{
    public interface IImagaMetadataProcessingService
    {
        ValueTask<ImageMetadata> StoreImageAsync(MemoryStream memoryStream, Guid clientId);
    }
}