//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using System.Linq;
using EasyPay.Api.Models.Accounts;
using FluentAssertions;
using Moq;
using Xunit;

namespace EasyPay.Api.Tests.Unit.Services.Foundations.Accounts
{
    public partial class AccountServiceTests
    {
        [Fact]
        public void ShouldRetrieveAllAccounts()
        {
            //given
            IQueryable<Account> randomAccounts = GetRandomAccounts();
            IQueryable<Account> persistedAccount = randomAccounts;
            IQueryable<Account> expectedAccounts = persistedAccount;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllAccounts()).Returns(expectedAccounts);

            //when
            IQueryable<Account> actualAccounts =
                this.accountService.RetrieveAllAccounts();

            //then
            actualAccounts.Should().BeEquivalentTo(expectedAccounts);

            this.storageBrokerMock.Verify(broker =>
            broker.SelectAllAccounts(), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
