//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using EasyPay.Api.Brokers.DateTimes;
using EasyPay.Api.Brokers.Loggings;
using EasyPay.Api.Brokers.Storages;
using EasyPay.Api.Models.Accounts;
using EasyPay.Api.Services.Foundations.Accounts;
using Microsoft.Data.SqlClient;
using Moq;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using Tynamix.ObjectFiller;
using Xeptions;

namespace EasyPay.Api.Tests.Unit.Services.Foundations.Accounts
{
    public partial class AccountServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IAccountService accountService;

        public AccountServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.accountService =
                new AccountService(
                    storageBroker: this.storageBrokerMock.Object,
                    dateTimeBroker: this.dateTimeBrokerMock.Object,
                    loggingBroker: this.loggingBrokerMock.Object);
        }

        private string GetRandomString() =>
            new MnemonicString().GetValue();

        private static Account CreateRandomAccount() =>
            CreateAccountFiller(date: GetRandomDateTimeOffset()).Create();

        private static IQueryable<Account> GetRandomAccounts()
        {
            return CreateAccountFiller(date: GetRandomDateTimeOffset())
                .Create(count: GetRandomNUmber()).AsQueryable();
        }

        private static int GetRandomNUmber() =>
            new IntRange(min: 2, max: 9).GetValue();

        private Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static Filler<Account> CreateAccountFiller(DateTimeOffset date)
        {
            var filler = new Filler<Account>();

            filler.Setup().OnType<DateTimeOffset>().Use(date);

            return filler;
        }
        private static SqlException GetSqlError() =>
           (SqlException)FormatterServices.GetUninitializedObject(typeof(SqlException));
    }
}
