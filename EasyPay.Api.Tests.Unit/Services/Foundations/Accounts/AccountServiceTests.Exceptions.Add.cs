//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using EasyPay.Api.Models.Accounts;
using EasyPay.Api.Models.Accounts.Exceptions;
using EasyPay.Api.Models.Clients.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
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
    }
}
