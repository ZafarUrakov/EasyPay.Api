//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using EasyPay.Api.Brokers.DateTimes;
using EasyPay.Api.Brokers.Loggings;
using EasyPay.Api.Brokers.Storages;
using EasyPay.Api.Services.Foundations.Accounts;
using EasyPay.Api.Services.Foundations.Clients;
using EasyPay.Api.Services.Foundations.ImagaMetadatas;
using EasyPay.Api.Services.Foundations.Transfers;
using EasyPay.Api.Services.Processings;
using EasyPay.Api.Services.Processings.Accounts;
using EasyPay.Api.Services.Processings.Clients;
using EasyPay.Api.Services.Processings.ImageMetadatas;
using EasyPay.Api.Services.Processings.Transfers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;

namespace EasyPay.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration) =>
            Configuration = configuration;

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
                });

            services.AddTransient<IStorageBroker, StorageBroker>();
            services.AddTransient<ILoggingBroker, LoggingBroker>();
            services.AddTransient<IDateTimeBroker, DateTimeBroker>();
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IClientService, ClientService>();
            services.AddTransient<ITransferService, TransferService>();
            services.AddTransient<IImageMetadataService, ImageMetadataService>();
            services.AddTransient<IClientProcessingService, ClientProcessingService>();
            services.AddTransient<IAccountProcessingService, AccountProcessingService>();
            services.AddTransient<ITransferProcessingService, TransferProcessingService>();
            services.AddTransient<IImagaMetadataProcessingService, ImagaMetadataProcessingService>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "EasyPay.Api", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "EasyPay.Api v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
