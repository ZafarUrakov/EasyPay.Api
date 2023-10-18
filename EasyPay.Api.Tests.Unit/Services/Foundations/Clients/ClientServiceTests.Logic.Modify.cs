﻿//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using EasyPay.Api.Models.Clients;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace EasyPay.Api.Tests.Unit.Services.Foundations.Clients
{
    public partial class ClientServiceTests
    {
        [Fact]
        public async Task ShouldModifyClientAsync()
        {
            // given 
            Client randomClient = CreateRandomClient();
            Client inputClient = randomClient;
            Client updatedClient = inputClient;
            Client expectedClient = updatedClient.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateClientAsync(inputClient))
                    .ReturnsAsync(updatedClient);

            //when
            Client actualClient =
                await this.clientService.ModifyClientAsync(inputClient);

            // then
            actualClient.Should().BeEquivalentTo(expectedClient);

            this.storageBrokerMock.Verify(broker =>
            broker.UpdateClientAsync(inputClient), Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
