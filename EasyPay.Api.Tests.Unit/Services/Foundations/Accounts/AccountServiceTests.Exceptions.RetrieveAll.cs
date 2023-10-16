//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using System;
using EasyPay.Api.Models.Accounts.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using Xunit;

namespace EasyPay.Api.Tests.Unit.Services.Foundations.Accounts
{
    public partial class AccountServiceTests
    {
        [Fact]
        public void ShouldThrowCriticalDependencyExceptionOnRetrieveAllIfSqlErrorOccureAndLogIt()
        {
            //given
            SqlException sqlException = GetSqlError();

            var failedStorageAccountException =
                new FailedStorageAccountException(sqlException);

            var expectedAccountDependencyException =
                new AccountDependencyException(failedStorageAccountException);

            this.storageBrokerMock.Setup(broker =>
            broker.SelectAllAccounts()).Throws(sqlException);

            //when
            Action retrieveAllAccountsAction = () =>
                this.accountService.RetrieveAllAccounts();

            AccountDependencyException accountDependencyException =
                Assert.Throws<AccountDependencyException>(retrieveAllAccountsAction);

            //then
            accountDependencyException.Should().BeEquivalentTo(
                expectedAccountDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllAccounts(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedAccountDependencyException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowServiceExceptionOnRetrieveAllIfServiceErrorOccursLogIt()
        {
            //given
            string someMessage = GetRandomString();
            var serviceException = new Exception(someMessage);

            var failedAccountServiceException =
                new FailedAccountServiceException(serviceException);

            var expectedAccountServiceException =
                new AccountServiceException(failedAccountServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllAccounts()).Throws(serviceException);

            //when
            Action retrieveAllAccountsAction = () =>
            this.accountService.RetrieveAllAccounts();

            AccountServiceException accountServiceException =
                Assert.Throws<AccountServiceException>(retrieveAllAccountsAction);

            //then
            accountServiceException.Should().BeEquivalentTo(expectedAccountServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllAccounts(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAccountServiceException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
