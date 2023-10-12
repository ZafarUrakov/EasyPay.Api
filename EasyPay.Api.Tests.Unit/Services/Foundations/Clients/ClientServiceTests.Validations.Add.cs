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

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnAddIfClientIsInvalidAndLogItAsync(
            string invalidText)
        {
            // given
            var invalidClient = new Client
            {
                FirstName = invalidText,
            };

            var invalidClientException = new InvalidClientException();

            invalidClientException.AddData(
                key: nameof(Client.ClientId),
                values: "Id is required");
            
            invalidClientException.AddData(
                key: nameof(Client.FirstName),
                values: "Text is required");
            
            invalidClientException.AddData(
                key: nameof(Client.LastName),
                values: "Text is required");

            invalidClientException.AddData(
                key: nameof(Client.BirthDate),
                values: "Date is required");

            invalidClientException.AddData(
                key: nameof(Client.Email),
                values: "Text is required");

            invalidClientException.AddData(
                key: nameof(Client.PhoneNumber),
                values: "Text is required");

            invalidClientException.AddData(
                key: nameof(Client.Address),
                values: "Text is required");

            var expectedClientValidationException = 
                new ClientValidationException(invalidClientException);

            // when
            ValueTask<Client> addClientTask = 
                this.clientService.AddClientAsync(invalidClient);

            ClientValidationException actualClientValidationException =
                await Assert.ThrowsAsync<ClientValidationException>(addClientTask.AsTask);

            // then
            actualClientValidationException.Should()
                .BeEquivalentTo(expectedClientValidationException);

            this.loggingBrokerMock.Verify(broker =>
            broker.LogError(It.Is(SameExceptionAs(
                expectedClientValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
            broker.InsertClientAsync(It.IsAny<Client>()), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
