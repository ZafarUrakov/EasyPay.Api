//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using EasyPay.Api.Models.Accounts;
using EasyPay.Api.Models.Accounts.Exceptions;
using EasyPay.Api.Models.Clients.Exceptions;
using EFxceptions.Models.Exceptions;
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
        public async Task ShouldThrowCriticalDependencyExceptionOnAddIfSqlErrorOccursLogItAsync()
        {
            //given
            Account someAccount = CreateRandomAccount();

            SqlException sqlException = GetSqlError();

            var failedAccountStorageException =
                new FailedStorageAccountException(sqlException);

            var expectedAccountDependencyException =
                new AccountDependencyException(failedAccountStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertAccountAsync(someAccount)).ThrowsAsync(sqlException);

            // when
            ValueTask<Account> addAccountTask =
                this.accountService.AddAccountAsync(someAccount);

            var actualAccountDependencyException =
                await Assert.ThrowsAsync<AccountDependencyException>(addAccountTask.AsTask);

            // then
            actualAccountDependencyException.Should()
                .BeEquivalentTo(expectedAccountDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAccountAsync(someAccount), Times.Once());

            this.loggingBrokerMock.Verify(broker =>
            broker.LogCritical(It.Is(SameExceptionAs(
                expectedAccountDependencyException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfDuplicateKeyErrorOccursAndLogItAsync()
        {
            // given 
            string someMessage = GetRandomString();
            Account someAccount = CreateRandomAccount();
            var duplicateKeyException = new DuplicateKeyException(someMessage);

            var alreadyExistsAccountException =
                new AlreadyExistsAccountException(duplicateKeyException);

            var expectedAccountDependencyValidationException =
                new AccountDependencyValidationException(alreadyExistsAccountException);

            this.storageBrokerMock.Setup(broker =>
            broker.InsertAccountAsync(someAccount)).ThrowsAsync(duplicateKeyException);

            // when
            ValueTask<Account> addApplicantTask =
                this.accountService.AddAccountAsync(someAccount);

            var actualaAccountDependencyValidationException = await Assert
                .ThrowsAsync<AccountDependencyValidationException>(addApplicantTask.AsTask);

            //then
            actualaAccountDependencyValidationException.Should()
                .BeEquivalentTo(expectedAccountDependencyValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAccountAsync(someAccount), Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAccountDependencyValidationException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddIfServiceErrorOccursAndLogItAsync()
        {
            // given
            Account someAccount = CreateRandomAccount();

            var serviceException = new Exception();

            var failedAccountServiceException =
                new FailedAccountServiceException(serviceException);

            var expectedAccountServiceException =
                new AccountServiceException(failedAccountServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertAccountAsync(someAccount)).ThrowsAsync(serviceException);

            // when
            ValueTask<Account> addAccountTask =
                this.accountService.AddAccountAsync(someAccount);

            AccountServiceException actualAccountServiceException = await Assert
                .ThrowsAsync<AccountServiceException>(addAccountTask.AsTask);
            // then
            actualAccountServiceException.Should()
                .BeEquivalentTo(expectedAccountServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAccountAsync(someAccount), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAccountServiceException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
