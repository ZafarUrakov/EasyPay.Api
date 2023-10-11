//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using System;

namespace EasyPay.Api.Brokers.Loggings
{
    public interface ILoggingBroker
    {
        void LogError(Exception exception);
        void LogCritical(Exception exception);
    }
}
