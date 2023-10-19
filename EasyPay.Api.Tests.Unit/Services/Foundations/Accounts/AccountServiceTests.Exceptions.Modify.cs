//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using System;
using System.Threading.Tasks;
using EasyPay.Api.Models.Accounts;
using EasyPay.Api.Models.Accounts.Exceptions;
using EasyPay.Api.Models.Clients.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
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

            var failedClientStorageException =
                new FailedClientStorageException(sqlException);

            var expectedAccountDependencyException =
                new AccountDependencyException(failedClientStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateAccountAsync(someAccount)).ThrowsAsync(sqlException);

            //when
            ValueTask<Account> modifyAccountTask =
                this.accountService.ModifyAccountAsync(someAccount);

            AccountDependencyException actualAccountDependencyException1 =
                await Assert.ThrowsAsync<AccountDependencyException>(modifyAccountTask.AsTask);

            //then
            actualAccountDependencyException1.Should().BeEquivalentTo(
                expectedAccountDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateAccountAsync(someAccount), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedAccountDependencyException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
