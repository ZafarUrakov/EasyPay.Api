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
        public async Task ShouldRemoveClientById()
        {
            // given
            Guid randomId = Guid.NewGuid();
            Guid inputClientId = randomId;
            Client randomClient = CreateRandomClient();
            Client storageClient = randomClient;
            Client expectedInputClient = storageClient;
            Client deletedClient = expectedInputClient;
            Client expectedClient = deletedClient.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectClientByIdAsync(inputClientId))
                    .ReturnsAsync(storageClient);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteClientAsync(expectedInputClient))
                    .ReturnsAsync(deletedClient);

            // when
            Client actualClient = await this
                .clientService.RemoveClientByIdAsync(randomId);

            // then 
            actualClient.Should().BeEquivalentTo(expectedClient);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectClientByIdAsync(inputClientId), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteClientAsync(expectedInputClient),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
