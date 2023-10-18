//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using EasyPay.Api.Models.Clients;
using EasyPay.Api.Models.Clients.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xunit;

namespace EasyPay.Api.Tests.Unit.Services.Foundations.Clients
{
    public partial class ClientServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnModifyIfSqlErrorOccursAndLogItAsync()
        {// given
            Client randomClient = CreateRandomClient();
            SqlException sqlException = GetSqlError();

            var failedClientStorageException =
                new FailedClientStorageException(sqlException);

            var expectedClientDependencyException =
                new ClientDependencyException(failedClientStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(sqlException);

            // when
            ValueTask<Client> modifyClientTask =
                this.clientService.ModifyClientAsync(randomClient);

            ClientDependencyException actualGroupDependencyException =
                 await Assert.ThrowsAsync<ClientDependencyException>(
                    modifyClientTask.AsTask);

            // then
            actualGroupDependencyException.Should().BeEquivalentTo(
                expectedClientDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectClientByIdAsync(randomClient.ClientId),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateClientAsync(randomClient),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedClientDependencyException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
