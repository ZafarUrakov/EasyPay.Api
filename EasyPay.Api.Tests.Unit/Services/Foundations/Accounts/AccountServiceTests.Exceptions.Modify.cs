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
        public async Task ShoulThrowCriticalDependencyExceptionOnModifyIfSqlErrorOccursAndLogItAsync()
        {
            //given
            Account randomAccount = CreateRandomAccount();
            Account someAccount = randomAccount;
            Guid accountId = someAccount.AccountId;
            SqlException sqlException = GetSqlError();

            var failedStorageAccountException =
                new FailedStorageAccountException(sqlException);

            var expectedAccountDependencyException =
                new AccountDependencyException(failedStorageAccountException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAccountByIdAsync(accountId)).ThrowsAsync(sqlException);

            //when
            ValueTask<Account> modifyAccountTask =
                this.accountService.ModifyAccountAsync(someAccount);

            AccountDependencyException actualAccountDependencyException1 =
                await Assert.ThrowsAsync<AccountDependencyException>(modifyAccountTask.AsTask);

            //then
            actualAccountDependencyException1.Should().BeEquivalentTo(
                expectedAccountDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAccountByIdAsync(accountId), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedAccountDependencyException))), Times.Once);

            //this.storageBrokerMock.Verify(broker =>
            //    broker.UpdateAccountAsync(It.IsAny<Account>()), Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnModifyifDatabaseUpdateConcurrencyErrorOccursAndLogItAsync()
        {
            //given
            Account randomAccount = CreateRandomAccount();
            Account someAccount = randomAccount;
            Guid accountId = someAccount.AccountId;
            var dbUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedAccountException = new LockedAccountException(dbUpdateConcurrencyException);

            var expectedAccountDependencyValidationException = new AccountDependencyValidationException(lockedAccountException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAccountByIdAsync(accountId)).ThrowsAsync(dbUpdateConcurrencyException);

            //when
            ValueTask<Account> modifyAccountTask = this.accountService.ModifyAccountAsync(someAccount);

            AccountDependencyValidationException actualAccountDependencyException =
                await Assert.ThrowsAsync<AccountDependencyValidationException>(modifyAccountTask.AsTask);

            //then
            actualAccountDependencyException.Should().BeEquivalentTo(expectedAccountDependencyValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAccountByIdAsync(accountId), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedAccountDependencyValidationException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnModifyIfDatabaseUpdateErrorOccursAndLogItAsync()
        {
            //given
            Account randomAccount = CreateRandomAccount();
            Account someAccount = randomAccount;
            Guid accountId = someAccount.AccountId;
            var dbUpdateException = new DbUpdateException();

            var failedStorageAccountException = new FailedStorageAccountException(dbUpdateException);

            var expectedAccountDependencyException =
                new AccountDependencyException(failedStorageAccountException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAccountByIdAsync(accountId)).ThrowsAsync(dbUpdateException);

            //when
            ValueTask<Account> modifyAccountTask = this.accountService.ModifyAccountAsync(someAccount);

            AccountDependencyException actualAccountDependencyException =
                await Assert.ThrowsAsync<AccountDependencyException>(modifyAccountTask.AsTask);

            //then
            actualAccountDependencyException.Should().BeEquivalentTo(expectedAccountDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAccountByIdAsync(accountId), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedAccountDependencyException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ThrowServiceExceptionOnModifyIfServiceErrorOccursAndLogItAsync()
        {
            //given
            Account randomAccount = CreateRandomAccount();
            Account someAccount = randomAccount;
            Guid accountId = someAccount.AccountId;
            var exception = new Exception();

            var failedAccountServiceException
                = new FailedAccountServiceException(exception);

            var expectedAccountServiceException =
                new AccountServiceException(failedAccountServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAccountByIdAsync(accountId)).ThrowsAsync(exception);

            //when
            ValueTask<Account> modifyAccountTask =
                this.accountService.ModifyAccountAsync(someAccount);

            AccountServiceException actualAccountService =
                await Assert.ThrowsAsync<AccountServiceException>(modifyAccountTask.AsTask);

            //then
            actualAccountService.Should().BeEquivalentTo(expectedAccountServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAccountByIdAsync(accountId), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAccountServiceException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
