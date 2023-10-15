//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

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
        public async Task ShouldAddAccountAsync()
        {
            //given
            Account randomAccount = CreateRandomAccount();
            Account inputAccount = randomAccount;
            Account persistedAccount = inputAccount;
            Account expectedAccount = persistedAccount.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.InsertAccountAsync(inputAccount))
                .ReturnsAsync(expectedAccount);

            //when
            Account actualAccount =
                await this.accountService.AddAccountAsync(inputAccount);

            //then
            actualAccount.Should().BeEquivalentTo(expectedAccount);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAccountAsync(inputAccount),
                Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();

        }
    }
}
