//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using EasyPay.Api.Models.Accounts;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace EasyPay.Api.Tests.Unit.Services.Foundations.Accounts
{
    public partial class AccountServiceTests
    {
        [Fact]
        public async Task ShouldModifyAccountAsync()
        {
            //given
            Account randomAccount = CreateRandomAccount();
            Account inputAccount = randomAccount;
            Account persistedAccount = inputAccount.DeepClone();
            Account updatedAccount = inputAccount;
            Account expectedAccount = updatedAccount.DeepClone();
            Guid accountId = inputAccount.AccountId;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAccountByIdAsync(accountId))
                    .ReturnsAsync(persistedAccount);

            this.storageBrokerMock.Setup(Brokers =>
                Brokers.UpdateAccountAsync(inputAccount))
                    .ReturnsAsync(updatedAccount);

            //when
            Account actualAccount =
                await this.accountService
                    .ModifyAccountAsync(inputAccount);

            //then
            actualAccount.Should().BeEquivalentTo(expectedAccount);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAccountByIdAsync(accountId), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateAccountAsync(inputAccount), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
