//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using EasyPay.Api.Models.Accounts;
using EasyPay.Api.Models.Accounts.Exceptions;
using FluentAssertions;
using Moq;
using System.Threading.Tasks;
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

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnAddIfApplicantIsInvalidAndLogItAsync(
            string invalidText)
        {
            //given
            var invalidAccount = new Account
            {
                Login = invalidText
            };

            var invalidAccountException = new InvalidAccountException();

            invalidAccountException.AddData(
                key: nameof(Account.AccountId),
                values: "Id is required");

            invalidAccountException.AddData(
                key: nameof(Account.AccountNumber),
                values: "Text is required");

            invalidAccountException.AddData(
                key: nameof(Account.Login),
                values: "Text is required");

            invalidAccountException.AddData(
                key: nameof(Account.Password),
                values: "Text is required");

            invalidAccountException.AddData(
                key: nameof(Account.ClientId),
                values: "Id is required");

            var expectedAccountValidationException =
                new AccountValidationException(invalidAccountException);

            //when
            ValueTask<Account> addAccountTask = this.accountService.AddAccountAsync(invalidAccount);

            AccountValidationException actualAccountValidationException =
                await Assert.ThrowsAsync<AccountValidationException>(addAccountTask.AsTask);

            //then
            actualAccountValidationException.Should().BeEquivalentTo(expectedAccountValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedAccountValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAccountAsync(It.IsAny<Account>()), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
