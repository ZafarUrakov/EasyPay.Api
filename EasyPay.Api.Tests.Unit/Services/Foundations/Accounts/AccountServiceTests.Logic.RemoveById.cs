//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using System;
using System.Threading.Tasks;
using EasyPay.Api.Models.Accounts;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using Xunit;

namespace EasyPay.Api.Tests.Unit.Services.Foundations.Accounts
{
    public partial class AccountServiceTests
    {
        [Fact]
        public async Task ShouldRemoveAccountByIdAsync()
        {
            // given
            Guid randomId = Guid.NewGuid();
            Guid inputId = randomId;
            Account randomAccount = CreateRandomAccount();
            Account persistedAccount = randomAccount;
            Account expectedInputAccount = persistedAccount;
            Account deleteAccount = expectedInputAccount;
            Account expectedAccount = deleteAccount.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAccountByIdAsync(inputId))
                    .ReturnsAsync(persistedAccount);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteAccountAsync(expectedAccount))
                    .ReturnsAsync(deleteAccount);

            // when
            Account actualAccount =
                await this.accountService
                    .RemoveAccountByIdAsync(inputId);

            // then
            actualAccount.Should().BeEquivalentTo(expectedAccount);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAccountByIdAsync(inputId), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteAccountAsync(
                    expectedInputAccount), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
