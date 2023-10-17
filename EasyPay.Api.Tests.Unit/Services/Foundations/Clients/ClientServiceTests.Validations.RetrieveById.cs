//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using EasyPay.Api.Models.Clients;
using EasyPay.Api.Models.Clients.Exceptions;
using FluentAssertions;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace EasyPay.Api.Tests.Unit.Services.Foundations.Clients
{
    public partial class ClientServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetrieveByIdIfIdIsInvalidAndLogItAsync()
        {
            // given
            Guid  invalidClientId = Guid.Empty;
            var invalidClientException = new InvalidClientException();

            invalidClientException.AddData(
                key: nameof(Client.ClientId),
                values: "Id is required");

            var expectedClientValidationException =
                new ClientValidationException(invalidClientException);

            // when
            ValueTask<Client> retrieveClientById = 
                this.clientService.RetrieveClientByIdAsync(invalidClientId);

            ClientValidationException actualClientValidationException =
                await Assert.ThrowsAsync<ClientValidationException>(retrieveClientById.AsTask);

            // then
            actualClientValidationException.Should().BeEquivalentTo(expectedClientValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedClientValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectClientByIdAsync(It.IsAny<Guid>()), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
