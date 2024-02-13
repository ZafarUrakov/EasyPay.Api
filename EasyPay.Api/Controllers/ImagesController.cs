//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using EasyPay.Api.Models.Images;
using EasyPay.Api.Services.Processings.ImageMetadatas;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;
using System;
using System.IO;
using System.Threading.Tasks;

namespace EasyPay.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImagesController : RESTFulController
    {
        private readonly IImagaMetadataProcessingService imagaMetadataProcessingService;

        public ImagesController(IImagaMetadataProcessingService imagaMetadataProcessingService) => 
            this.imagaMetadataProcessingService = imagaMetadataProcessingService;

        [HttpPost]
        public async ValueTask<ActionResult<ImageMetadata>> PostImageMetadataAsync([FromHeader]IFormFile imageFile, [FromHeader] Guid clientId)
        {
            ImageMetadata imageMetadata;

            using (var memoryStream = new MemoryStream())
            {
                imageFile.CopyTo(memoryStream);

                imageMetadata = await this.imagaMetadataProcessingService.StoreImageAsync(memoryStream, clientId);
            }

            return Created(imageMetadata);
        }
    }
}
