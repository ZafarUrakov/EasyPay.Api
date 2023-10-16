//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using System;
using System.Threading.Tasks;
using EasyPay.Api.Models.Accounts;
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
    }
}
