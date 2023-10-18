//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using System;
using System.Threading.Tasks;
using EasyPay.Api.Models.Accounts;
using EasyPay.Api.Models.Accounts.Exceptions;
using FluentAssertions;
using Moq;
using Xunit;

namespace EasyPay.Api.Tests.Unit.Services.Foundations.Accounts
{
    public partial class AccountServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRemoveByIdIfIdIsInvalidAndLogItAsync()
        {
            //given
            Guid invalidAccountId = Guid.Empty;

            var invalidAccountException = new InvalidAccountException();

            invalidAccountException.AddData(
                key: nameof(Account.AccountId),
                values: "Id is required");

            var expectedAccountValidationException =
                new AccountValidationException(invalidAccountException);

            //when
            ValueTask<Account> removeAccountByIdTask =
                this.accountService.RemoveAccountByIdAsync(invalidAccountId);

            AccountValidationException actualAccountValidationException =
                await Assert.ThrowsAsync<AccountValidationException>(removeAccountByIdTask.AsTask);

            //then
            actualAccountValidationException.Should().BeEquivalentTo(expectedAccountValidationException);

            this.loggingBrokerMock.Verify(broker => broker.LogError(It.Is(SameExceptionAs(
                    expectedAccountValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteAccountAsync(It.IsAny<Account>()), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
