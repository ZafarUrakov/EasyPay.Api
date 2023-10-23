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
        public async Task ShouldThrowValidationExceptionIfAccountIsNullAndLogItAsync()
        {
            //given
            Account nullAccount = null;
            var nullAccountException = new NullAccountException();

            var expectedAccountValidationException =
                new AccountValidationException(nullAccountException);

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

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnModifyAccountIfAccountIsInvalidAndLogItAsync(
            string invalidString)
        {
            //given
            var invalidAccount = new Account
            {
                Login = invalidString
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
            ValueTask<Account> modifyAccounttask =
                this.accountService.ModifyAccountAsync(invalidAccount);

            AccountValidationException actualAccountValidationException =
                await Assert.ThrowsAsync<AccountValidationException>(
                    modifyAccounttask.AsTask);

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

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfAccountDoesNOtExistsAndLogItAsync()
        {
            //given
            Account randomAccount = CreateRandomAccount();
            Account nonExistsAccount = randomAccount;
            Account nullAccount = null;

            var notFoundAccountException =
                new NotFoundAccountException(nonExistsAccount.AccountId);

            var expectedAccountValidationException =
                new AccountValidationException(notFoundAccountException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAccountByIdAsync(nonExistsAccount.AccountId)).ReturnsAsync(nullAccount);

            //when
            ValueTask<Account> modifyAccountTask =
                this.accountService.ModifyAccountAsync(nonExistsAccount);

            AccountValidationException actualAccountValidationException =
                await Assert.ThrowsAsync<AccountValidationException>(modifyAccountTask.AsTask);

            //then
            actualAccountValidationException.Should().BeEquivalentTo(expectedAccountValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAccountByIdAsync(nonExistsAccount.AccountId), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedAccountValidationException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
