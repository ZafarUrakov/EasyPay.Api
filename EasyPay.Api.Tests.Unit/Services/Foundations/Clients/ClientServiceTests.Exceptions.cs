//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using EasyPay.Api.Models.Clients;
using EasyPay.Api.Models.Clients.Exceptions;
using EFxceptions.Models.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace EasyPay.Api.Tests.Unit.Services.Foundations.Clients
{
    public partial class ClientServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnAddIfSqlErrorOccursLogItAsync()
        {
            // given
            Client someClient = CreateRandomClient();

            SqlException sqlException = GetSqlError();
            var failedClientStorageException = 
                new FailedClientStorageException(sqlException);

            var expectedClientDependencyException = 
                new ClientDependencyException(failedClientStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertClientAsync(someClient))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<Client> addClientTask =
                this.clientService.AddClientAsync(someClient);

            // then
            await Assert.ThrowsAsync<ClientDependencyException>(() =>
                addClientTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.InsertClientAsync(someClient), Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedClientDependencyException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfDuplicateKeyErrorOccursAndLogItAsync()
        {
            // given
            string someMessage = GetRandomString();
            Client someClient = CreateRandomClient();

            DuplicateKeyException duplicateKeyException = new DuplicateKeyException(someMessage);

            var alreadyExistsClientException = 
                new AlreadyExistsClientException(duplicateKeyException);

            var expectedClientDependencyValidationException = 
                new ClientDependencyValidationException(alreadyExistsClientException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertClientAsync(someClient)).ThrowsAsync(duplicateKeyException);
            // when
            ValueTask<Client> addClientTask =
                this.clientService.AddClientAsync(someClient);

            var actualClientDependencyValidationException =
                 await Assert.ThrowsAsync<ClientDependencyValidationException>(addClientTask.AsTask);

            // then
            actualClientDependencyValidationException.Should()
                .BeEquivalentTo(expectedClientDependencyValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertClientAsync(someClient), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedClientDependencyValidationException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
