//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using EasyPay.Api.Models.Clients;
using EasyPay.Api.Models.Clients.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace EasyPay.Api.Tests.Unit.Services.Foundations.Clients
{
    public partial class ClientServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnModifyIfSqlErrorOccursAndLogItAsync()
        {
            // given
            Client randomClient = CreateRandomClient();
            Client someClient = randomClient;
            Guid clientId = someClient.ClientId;
            SqlException sqlException = GetSqlError();

            var failedClientStorageException =
                new FailedClientStorageException(sqlException);

            var expectedClientDependencyException =
                new ClientDependencyException(failedClientStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectClientByIdAsync(clientId)).Throws(sqlException);

            // when
            ValueTask<Client> modifyClientTask =
                this.clientService.ModifyClientAsync(someClient);

            ClientDependencyException actualClientDependencyException =
                 await Assert.ThrowsAsync<ClientDependencyException>(
                    modifyClientTask.AsTask);

            // then
            actualClientDependencyException.Should()
                .BeEquivalentTo(expectedClientDependencyException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedClientDependencyException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectClientByIdAsync(clientId), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateClientAsync(someClient), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnModifyIfDatabaseUpdateExceptionOccursAndLogItAsync()
        {
            // given
            Client randomClient = CreateRandomClient();
            Client someClient = randomClient;
            Guid clientId = someClient.ClientId;
            var databaseUpdateException = new DbUpdateException();

            var failedClientStorageException =
                new FailedClientStorageException(databaseUpdateException);

            var expectedClientDependencyException =
                new ClientDependencyException(failedClientStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectClientByIdAsync(clientId)).Throws(databaseUpdateException);

            // when
            ValueTask<Client> modifyClientTask =
                this.clientService.ModifyClientAsync(someClient);

            ClientDependencyException actualClientDependencyException =
                 await Assert.ThrowsAsync<ClientDependencyException>(
                    modifyClientTask.AsTask);

            // then
            actualClientDependencyException.Should()
                .BeEquivalentTo(expectedClientDependencyException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedClientDependencyException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectClientByIdAsync(clientId), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateClientAsync(someClient), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnModifyIfDatabaseUpdateConcurrencyErrorOccursAndLogItAsync()
        {
            // given
            Client randomClient = CreateRandomClient();
            Client someClient = randomClient;
            Guid clientId = someClient.ClientId;
            var dbUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedClientException =
                new LockedClientException(dbUpdateConcurrencyException);

            var expectedClientDependencyValidatoinException =
                new ClientDependencyValidationException(lockedClientException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectClientByIdAsync(clientId))
                    .Throws(dbUpdateConcurrencyException);

            // when
            ValueTask<Client> modifyClientTask =
                this.clientService.ModifyClientAsync(someClient);

            ClientDependencyValidationException actualClientDependencyValidationException =
                 await Assert.ThrowsAsync<ClientDependencyValidationException>(
                    modifyClientTask.AsTask);

            // then
            actualClientDependencyValidationException.Should()
                .BeEquivalentTo(expectedClientDependencyValidatoinException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedClientDependencyValidatoinException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectClientByIdAsync(clientId), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateClientAsync(someClient), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
