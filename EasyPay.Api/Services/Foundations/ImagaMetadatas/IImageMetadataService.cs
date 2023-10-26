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
        ValueTask<ImageMetadata> AddClientAsync(ImageMetadata imageMetadata);
        IQueryable<ImageMetadata> RetrieveAllClients();
        ValueTask<ImageMetadata> RetrieveClientByIdAsync(Guid clientId);
        ValueTask<ImageMetadata> RemoveClientByIdAsync(Guid locationId);
        ValueTask<ImageMetadata> ModifyClientAsync(ImageMetadata imageMetadata);
    }
}