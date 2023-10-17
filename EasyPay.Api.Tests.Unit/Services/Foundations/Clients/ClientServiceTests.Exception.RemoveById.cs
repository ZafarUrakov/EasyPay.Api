//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using EasyPay.Api.Models.Clients;
using EasyPay.Api.Models.Clients.Exceptions;
using FluentAssertions;
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
        public async Task ShouldThrowDependencyValidationOnRemoveIfDatabaseUpdateConcurrencyErrorOccursAndLogItAsync()
        {
            // given
            Guid someClientId = Guid.NewGuid();

            var dbUpdateConcurrencyException =
                new DbUpdateConcurrencyException();

            var lockedClientException =
                new LockedClientException(dbUpdateConcurrencyException);

            var expectedClientDependencyValidationException =
                new ClientDependencyValidationException(lockedClientException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectClientByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(dbUpdateConcurrencyException);

            // when
            ValueTask<Client> removeClientById =
                this.clientService.RemoveClientById(someClientId);

            var actualClientDependencyValidationException =
                await Assert.ThrowsAsync<ClientDependencyValidationException>(
                    removeClientById.AsTask);

            // then
            actualClientDependencyValidationException.Should()
                .BeEquivalentTo(expectedClientDependencyValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectClientByIdAsync(It.IsAny<Guid>()), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedClientDependencyValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteClientAsync(It.IsAny<Client>()), Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
