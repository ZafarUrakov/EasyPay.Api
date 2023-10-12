//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using EasyPay.Api.Models.Clients;
using EasyPay.Api.Models.Clients.Exceptions;
using FluentAssertions;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace EasyPay.Api.Tests.Unit.Services.Foundations.Clients
{
    public partial class ClientServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfInputIsNullAndLogItAsync()
        {
            // given
            Client nullClient = null;
            var nullClientException = new NullClientException();

            var expectedClientValidationException = 
                new ClientValidationException(nullClientException);

            // when
            ValueTask<Client> addClientTask = 
                this.clientService.AddClientAsync(nullClient);

            ClientValidationException actualClientException = 
                await Assert.ThrowsAsync<ClientValidationException>(addClientTask.AsTask);

            // then
            actualClientException.Should().BeEquivalentTo(expectedClientValidationException);

            this.loggingBrokerMock.Verify(broker =>
            broker.LogError(It.Is
            (SameExceptionAs(expectedClientValidationException))), 
                Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertClientAsync(It.IsAny<Client>()), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
