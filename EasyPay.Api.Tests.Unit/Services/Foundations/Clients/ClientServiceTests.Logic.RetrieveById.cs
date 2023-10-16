//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using EasyPay.Api.Models.Clients;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace EasyPay.Api.Tests.Unit.Services.Foundations.Clients
{
    public partial class ClientServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveClientByIdAsync()
        {
            // given
            Guid randomClientId = Guid.NewGuid();
            Guid inputClientId = randomClientId;
            Client randomClient = CreateRandomClient();
            Client persistedClient = randomClient;
            Client expectedClient = persistedClient.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectClientByIdAsync(inputClientId))
                    .ReturnsAsync(persistedClient);

            // when
            Client actualClient = await this
                .clientService.RetrieveClientByIdAsync(inputClientId);

            // then 
            actualClient.Should().BeEquivalentTo(expectedClient);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectClientByIdAsync(inputClientId), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
