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
        public async Task ShouldThrowValidationExceptionIfAccountIsNullAndLogItAsync()
        {
            //given
            Account nullAccount = null;

            var nullAccountException = new NullAccountException();

            var expectedAccountValidationException =
                new AccountValidationException(nullAccountException);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateAccountAsync(nullAccount))
                    .ThrowsAsync(nullAccountException);

            //when
            ValueTask<Account> modifyAccountTask =
                this.accountService.ModifyAccountAsync(nullAccount);

            AccountValidationException actualAccountValidationException =
                await Assert.ThrowsAsync<AccountValidationException>(
                    modifyAccountTask.AsTask);

            //then
            actualAccountValidationException.Should().BeEquivalentTo(
                expectedAccountValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedAccountValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateAccountAsync(It.IsAny<Account>()), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
