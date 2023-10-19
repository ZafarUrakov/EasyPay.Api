//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using EasyPay.Api.Models.Accounts;
using EasyPay.Api.Models.Accounts.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace EasyPay.Api.Tests.Unit.Services.Foundations.Accounts
{
    public partial class AccountServiceTests
    {
        [Fact]
        public async Task ShouldThrowSqlExceptionOnRetrieveByIdIfSqlErrorOccursAndLogItAsync()
        {
            //given
            Guid someId = Guid.NewGuid();
            SqlException sqlException = GetSqlError();

            var failedStorageAccountException =
                new FailedStorageAccountException(sqlException);

            var expectedAccountDependencyException = new
                AccountDependencyException(failedStorageAccountException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAccountByIdAsync(It.IsAny<Guid>())).ThrowsAsync(sqlException);

            //when
            ValueTask<Account> retrieveAccountByIdTask =
                this.accountService.RetrieveAccountByIdAsync(someId);

            AccountDependencyException actualAccountDependencyException =
                await Assert.ThrowsAsync<AccountDependencyException>(retrieveAccountByIdTask.AsTask);

            //then
            actualAccountDependencyException.Should().BeEquivalentTo(expectedAccountDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAccountByIdAsync(It.IsAny<Guid>()), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedAccountDependencyException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveByIdIfServiceErrorOccursAndLogItAsync()
        {
            //given
            Guid someId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedAccountServiceException = new FailedAccountServiceException(serviceException);

            var expectedAccountServiceException =
                new AccountServiceException(failedAccountServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAccountByIdAsync(It.IsAny<Guid>())).ThrowsAsync(serviceException);

            //when
            ValueTask<Account> retrieveAccountByIdTask =
                this.accountService.RetrieveAccountByIdAsync(someId);

            AccountServiceException actualAccountServiceException =
                await Assert.ThrowsAsync<AccountServiceException>(retrieveAccountByIdTask.AsTask);

            //then
            actualAccountServiceException.Should().BeEquivalentTo(expectedAccountServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAccountByIdAsync(It.IsAny<Guid>()), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedAccountServiceException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
