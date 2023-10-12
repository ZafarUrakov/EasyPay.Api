//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using System;
using Xeptions;

namespace EasyPay.Api.Models.Accounts.Exceptions
{
    public class FailedAccountServiceException : Xeption
    {
        public FailedAccountServiceException(Exception innerException)
            : base(message: "Failed account service error occurred, contact support",
                  innerException)
        { }
    }
}
