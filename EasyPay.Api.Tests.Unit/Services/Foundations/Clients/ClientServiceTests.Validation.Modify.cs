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
        public async Task ShouldThrowValidationExceptionOnModifyIfClientIsNullAndLogItAsync()
        {
            // given
            Client nullClient = null;
            var nullClientException = new NullClientException();

            var expectedClientValidationException =
                new ClientValidationException(nullClientException);

            // when
            ValueTask<Client> modifyClientTask =
                this.clientService.ModifyClientAsync(nullClient);

            ClientValidationException actualClientValidationException =
                await Assert.ThrowsAsync<ClientValidationException>(
                    modifyClientTask.AsTask);

            // then
            actualClientValidationException.Should()
                .BeEquivalentTo(expectedClientValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedClientValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateClientAsync(It.IsAny<Client>()), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnModifyIfClientIsInvalidAndLogItAsync(string invalidString)
        {
            //given
            Client invalidClient = new Client
            {
                FirstName = invalidString
            };

            var invalidClientException =
                new InvalidClientException();

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
                key: nameof(Client.PhoneNumber),
                values: "Text is required");

            invalidClientException.AddData(
                key: nameof(Client.Email),
                values: "Text is required");

            invalidClientException.AddData(
                key: nameof(Client.Address),
                values: "Text is required");

            var expectedClientValidationException =
                new ClientValidationException(invalidClientException);

            // when
            ValueTask<Client> modifyClientTask =
                this.clientService.ModifyClientAsync(invalidClient);

            ClientValidationException actualClientValidationException =
                await Assert.ThrowsAsync<ClientValidationException>(
                    modifyClientTask.AsTask);

            // then
            actualClientValidationException.Should()
                .BeEquivalentTo(expectedClientValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedClientValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateClientAsync(It.IsAny<Client>()), Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfClientDoesNotExistAndLogItAsync()
        {
            // given
            Client randomClient = CreateRandomClient();
            Client nonExistClient = randomClient;
            Client nullClient = null;

            var notFoundClientException =
                new NotFoundClientException(nonExistClient.ClientId);

            var expectedClientValidationException =
                new ClientValidationException(notFoundClientException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectClientByIdAsync(nonExistClient.ClientId))
                    .ReturnsAsync(nullClient);

            // when
            ValueTask<Client> modifyClientTask =
                this.clientService.ModifyClientAsync(nonExistClient);

            ClientValidationException actualClientValidationException =
                await Assert.ThrowsAsync<ClientValidationException>
                    (modifyClientTask.AsTask);

            // then
            actualClientValidationException.Should().BeEquivalentTo(expectedClientValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectClientByIdAsync(nonExistClient.ClientId), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedClientValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateClientAsync(nonExistClient), Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
