//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using System;
using Xeptions;

namespace EasyPay.Api.Models.Clients.Exceptions
{
    public class FailedServiceStrageException : Xeption
    {
        public FailedServiceStrageException(Exception innerException)
            : base(message: "Failed client storage error occurred, contact support.", innerException)
        { }
    }
}
