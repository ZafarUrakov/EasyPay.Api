//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using Microsoft.Extensions.Logging;
using System;

namespace EasyPay.Api.Brokers.Loggings
{
    public class LoggingBroker : ILoggingBroker
    {
        private readonly ILogger<LoggingBroker> logger;
        
        public LoggingBroker(ILogger<LoggingBroker> logger) =>
            this.logger = logger;

        public void LogCritical(Exception exception) =>
            this.logger.LogError(exception.Message, exception);

        public void LogError(Exception exception) =>
            this.logger.LogCritical(exception.Message, exception);
    }
}
