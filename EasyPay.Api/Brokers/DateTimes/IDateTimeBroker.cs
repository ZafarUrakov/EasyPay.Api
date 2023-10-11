//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using System;

namespace EasyPay.Api.Brokers.DateTimes
{
    public interface IDateTimeBroker
    {
        DateTimeOffset GetCurrentDateTimeOffset();
    }
}
