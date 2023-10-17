﻿//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using EasyPay.Api.Models.Clients;
using EasyPay.Api.Models.Clients.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace EasyPay.Api.Tests.Unit.Services.Foundations.Clients
{
    public partial class ClientServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnRetrieveByIdIfSqlErrorOccursAndLogItAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            SqlException sqlException = GetSqlError();

            var failedClientStorageException = 
                new FailedClientStorageException(sqlException);

            var expectedClientDependencyException =
                new ClientDependencyException(failedClientStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectClientByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            // when 
            ValueTask<Client> retrieveClientById =
                this.clientService.RetrieveClientByIdAsync(someId);

            ClientDependencyException actualClientDependencyException =
                await Assert.ThrowsAsync<ClientDependencyException>(
                    retrieveClientById.AsTask);

            // then
            actualClientDependencyException.Should()
                .BeEquivalentTo(expectedClientDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectClientByIdAsync(someId), Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedClientDependencyException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
