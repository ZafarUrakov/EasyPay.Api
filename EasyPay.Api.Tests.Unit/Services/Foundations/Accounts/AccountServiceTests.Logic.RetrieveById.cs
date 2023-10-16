//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using System;
using System.Threading.Tasks;
using EasyPay.Api.Models.Accounts;
using FluentAssertions;
using Moq;
using Xunit;

namespace EasyPay.Api.Tests.Unit.Services.Foundations.Accounts
{
    public partial class AccountServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveAccountByIdAsync()
        {
            //given
            Guid randomAccountId = Guid.NewGuid();
            Guid inputAccountId = randomAccountId;
            Account randomAccount = CreateRandomAccount();
            Account persistedAccount = randomAccount;
            Account expectedAccount = persistedAccount;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAccountByIdAsync(inputAccountId)).ReturnsAsync(persistedAccount);

            //when
            Account actualAccount = await this.accountService.RetrieveAccountByIdAsync(inputAccountId);

            //then
            actualAccount.Should().BeEquivalentTo(expectedAccount);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAccountByIdAsync(inputAccountId), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
