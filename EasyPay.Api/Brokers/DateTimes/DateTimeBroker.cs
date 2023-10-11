//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using System;

namespace EasyPay.Api.Brokers.DateTimes
{
    public class DateTimeBroker : IDateTimeBroker
    {
        public DateTimeOffset GetCurrentDateTimeOffset() =>
            DateTimeOffset.UtcNow;
    }
}
