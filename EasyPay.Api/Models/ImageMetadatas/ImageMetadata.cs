//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using EasyPay.Api.Models.Clients;
using System;

namespace EasyPay.Api.Models.Images
{
    public class ImageMetadata
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public float Size { get; set; }
        public ImageType Type { get; set; }
        public Guid ClientId { get; set; }
        public Client Client { get; set; }
    }
}
