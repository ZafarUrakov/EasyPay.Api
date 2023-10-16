//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using EasyPay.Api.Models.Clients;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using System.Linq;
using Xunit;

namespace EasyPay.Api.Tests.Unit.Services.Foundations.Clients
{
    public partial class ClientServiceTests
    {
        [Fact]
        public void ShouldRetrieveAllClients()
        {
            // given
            IQueryable<Client> randomClients = CreateRandomClients();
            IQueryable<Client> persistedClients = randomClients;
            IQueryable<Client> expectedClients = persistedClients.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllClients()).Returns(persistedClients);

            // when
            IQueryable<Client> actualClients =
                this.clientService.RetrieveAllClients();

            // then
            actualClients.Should().BeEquivalentTo(expectedClients);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllClients(), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
