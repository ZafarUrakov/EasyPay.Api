//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using System;
using System.Threading.Tasks;
using EasyPay.Api.Models.Accounts;
using EasyPay.Api.Models.Accounts.Exceptions;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
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
    }
}
