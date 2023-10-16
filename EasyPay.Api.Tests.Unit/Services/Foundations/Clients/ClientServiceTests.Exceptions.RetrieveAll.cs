//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using EasyPay.Api.Models.Clients.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using System;
using Xunit;

namespace EasyPay.Api.Tests.Unit.Services.Foundations.Clients
{
    public partial class ClientServiceTests
    {
        [Fact]
        public void ShouldThrowCriticalDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
        {
            // given 
            SqlException sqlException = GetSqlError();

            var failedClientStorageException = 
                new FailedClientStorageException(sqlException);

            var expectedClientDependencyException = 
                new ClientDependencyException(failedClientStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllClients()).Throws(sqlException);

            // when
            Action retrieveAllClientsAction = () =>
                this.clientService.RetrieveAllClients();

            ClientDependencyException actualClientDependencyException = 
                Assert.Throws<ClientDependencyException>(retrieveAllClientsAction);

            // then
            actualClientDependencyException.Should()
                .BeEquivalentTo(expectedClientDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllClients(), Times.Once());

            this.loggingBrokerMock.Verify(broker =>
            broker.LogCritical(It.Is(SameExceptionAs(
                expectedClientDependencyException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowServiceExceptionOnRetrieveAllIfServiceErrorOccursAndLogItAsync()
        {
            // given
            string exceptionMessage = GetRandomString();
            var serverException = new Exception(exceptionMessage);

            var failedClientServiceException = 
                new FailedClientServiceException(serverException);

            var expectedClientServiceException = 
                new ClientServiceException(failedClientServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllClients()).Throws(serverException);

            // when 
            Action retrieveAllclientActions = () =>
                this.clientService.RetrieveAllClients();

            ClientServiceException actualClientServiceException = 
                Assert.Throws<ClientServiceException>(retrieveAllclientActions);

            // when
            actualClientServiceException.Should()
                .BeEquivalentTo(expectedClientServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllClients(), Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedClientServiceException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
