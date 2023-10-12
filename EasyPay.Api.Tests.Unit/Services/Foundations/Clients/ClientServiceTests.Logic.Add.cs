//===========================
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
        public async Task ShouldAddClientAsync()
        {
            // given 
            Client randomClient = CreateRandomClient();
            Client inputClient = randomClient;
            Client persistedClient = inputClient;
            Client expectedClient = persistedClient.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.InsertClientAsync(inputClient))
                    .ReturnsAsync(persistedClient);

            // when
            Client actualClient = await this.clientService
                .AddClientAsync(inputClient);

            // then
            actualClient.Should().BeEquivalentTo(expectedClient);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertClientAsync(inputClient), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
