//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

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
        public async Task ShouldThrowValidationExceptionOnAddIfInputIsNullAndLogItAsync()
        {
            //given
            Account nullAccount = null;
            var nullAccountException = new NullAccountException();

            var expectedAccountValidationException =
                new AccountValidationException(nullAccountException);

            //when
            ValueTask<Account> addAccountTask =
                this.accountService.AddAccountAsync(nullAccount);

            AccountValidationException actualAccountValidationException =
                await Assert.ThrowsAsync<AccountValidationException>(() =>
                    addAccountTask.AsTask());

            //then
            actualAccountValidationException.Should().BeEquivalentTo(
                expectedAccountValidationException);

            this.loggingBrokerMock.Verify(broker =>
            broker.LogError(It.Is(SameExceptionAs(
                expectedAccountValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAccountAsync(It.IsAny<Account>()), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
