//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using EasyPay.Api.Models.Images;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EasyPay.Api.Services.Foundations.ImagaMetadatas
{
    public interface IImageMetadataService
    {
        ValueTask<ImageMetadata> AddClientAsync(ImageMetadata imageMetadata);
        IQueryable<ImageMetadata> RetrieveAllClients();
        ValueTask<ImageMetadata> RetrieveClientByIdAsync(Guid clientId);
        ValueTask<ImageMetadata> RemoveClientByIdAsync(Guid locationId);
        ValueTask<ImageMetadata> ModifyClientAsync(ImageMetadata imageMetadata);
    }
}