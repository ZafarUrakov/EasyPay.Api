//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using System;
using Xeptions;

namespace EasyPay.Api.Models.Clients.Exceptions
{
    public class FailedClientServiceException : Xeption
    {
        public FailedClientServiceException(Exception innerException)
            : base(message: "Failed job service error occurred, please contact support.", innerException)
        { }
    }
}
