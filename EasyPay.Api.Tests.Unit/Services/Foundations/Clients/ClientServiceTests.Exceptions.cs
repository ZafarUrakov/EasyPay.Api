//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using EasyPay.Api.Models.Clients;
using EasyPay.Api.Models.Clients.Exceptions;
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
    }
}
