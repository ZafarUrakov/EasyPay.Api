//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using EasyPay.Api.Models.Accounts;
using EasyPay.Api.Models.Accounts.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace EasyPay.Api.Tests.Unit.Services.Foundations.Accounts
{
    public partial class AccountServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyValidationOnRemoveByIdIdDependencyConccurencyErrorOccursAndLogItAsync()
        {
            //given
            Guid someAccountId = Guid.NewGuid();
            Guid inputAccountId = someAccountId;

            var dbUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedAccountException =
                new LockedAccountException(dbUpdateConcurrencyException);

            var expectedAccountDependencyValidationException =
                new AccountDependencyValidationException(lockedAccountException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAccountByIdAsync(It.IsAny<Guid>())).ThrowsAsync(dbUpdateConcurrencyException);

            //when
            ValueTask<Account> removeAccountByIdTask =
                this.accountService.RemoveAccountByIdAsync(inputAccountId);

            AccountDependencyValidationException actualAccountDependencyValidationException =
                await Assert.ThrowsAsync<AccountDependencyValidationException>(removeAccountByIdTask.AsTask);

            //then
            actualAccountDependencyValidationException.Should().BeEquivalentTo(
                expectedAccountDependencyValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAccountByIdAsync(It.IsAny<Guid>()), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAccountDependencyValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteAccountAsync(It.IsAny<Account>()), Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowSqlExceptionOnRemoveByIdIfSqlErrorOccursAndLogItAsync()
        {
            //given
            Guid someAccountId = Guid.NewGuid();
            Guid inputAccountId = someAccountId;

            SqlException sqlException = GetSqlError();

            var failedStorageAccountException =
                new FailedStorageAccountException(sqlException);

            AccountDependencyException expectedAccountDependencyException =
                new AccountDependencyException(failedStorageAccountException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAccountByIdAsync(It.IsAny<Guid>())).ThrowsAsync(sqlException);

            //when
            ValueTask<Account> removeAccountByIdTask =
                this.accountService.RemoveAccountByIdAsync(inputAccountId);

            AccountDependencyException actualAccountDependencyException =
                await Assert.ThrowsAsync<AccountDependencyException>(
                    removeAccountByIdTask.AsTask);

            //then
            actualAccountDependencyException.Should().BeEquivalentTo(
                expectedAccountDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAccountByIdAsync(It.IsAny<Guid>()), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedAccountDependencyException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRemoveByIdIfServiceErrorOccursAndLogItAsync()
        {
            //given
            Guid someAccountid = Guid.NewGuid();
            Guid inputAccountid = someAccountid;

            Exception exception = new Exception();

            var failedAccountServiceException =
                new FailedAccountServiceException(exception);

            var expectedAccountServiceException =
                new AccountServiceException(failedAccountServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAccountByIdAsync(It.IsAny<Guid>())).ThrowsAsync(exception);

            //when
            ValueTask<Account> removeAccountByIdTask =
                this.accountService.RemoveAccountByIdAsync(inputAccountid);

            AccountServiceException actualAccountServiceException =
                await Assert.ThrowsAsync<AccountServiceException>(removeAccountByIdTask.AsTask);

            //then
            actualAccountServiceException.Should().BeEquivalentTo(expectedAccountServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAccountByIdAsync(It.IsAny<Guid>()), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAccountServiceException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
