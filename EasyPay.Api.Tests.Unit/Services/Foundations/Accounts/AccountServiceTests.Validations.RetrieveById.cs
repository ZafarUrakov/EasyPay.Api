//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using EasyPay.Api.Models.Accounts;
using EasyPay.Api.Models.Accounts.Exceptions;
using FluentAssertions;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace EasyPay.Api.Tests.Unit.Services.Foundations.Accounts
{
    public partial class AccountServiceTests
    {
        [Fact]
        public async Task ShouldTrowValidationExceptionOnRetrieveByIdIfIdIsInvalidAndLogItAsync()
        {
            //given
            Guid invalidAccountId = Guid.Empty;
            var invalidAccountException = new InvalidAccountException();

            invalidAccountException.AddData(
                key: nameof(Account.AccountId),
                values: "Id is required");

            var expectedAccountValidationException = new AccountValidationException(invalidAccountException);

            //when
            ValueTask<Account> retrieveAccountByIdTask =
                this.accountService.RetrieveAccountByIdAsync(invalidAccountId);

            AccountValidationException accountValidationException =
                await Assert.ThrowsAsync<AccountValidationException>(retrieveAccountByIdTask.AsTask);

            //then
            accountValidationException.Should().BeEquivalentTo(expectedAccountValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedAccountValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAccountByIdAsync(It.IsAny<Guid>()), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionOnRetrieveByIdIfAccountIsNotFoundAndLogItAsync()
        {
            //given
            Guid someId = Guid.NewGuid();
            Account noAccount = null;

            var notFoundAccountException = new NotFoundAccountException(someId);

            var expectedAccountValidationException =
                new AccountValidationException(notFoundAccountException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAccountByIdAsync(It.IsAny<Guid>())).ReturnsAsync(noAccount);

            //when
            ValueTask<Account> retrieveAccountByIdTask =
                this.accountService.RetrieveAccountByIdAsync(someId);

            AccountValidationException accountValidationException =
                await Assert.ThrowsAsync<AccountValidationException>(retrieveAccountByIdTask.AsTask);

            //then
            accountValidationException.Should().BeEquivalentTo(expectedAccountValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAccountByIdAsync(It.IsAny<Guid>()), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedAccountValidationException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
