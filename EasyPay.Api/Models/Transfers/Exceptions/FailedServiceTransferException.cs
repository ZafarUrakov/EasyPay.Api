//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using System;
using Xeptions;

namespace EasyPay.Api.Models.Transfers.Exceptions
{
    public class FailedServiceTransferException : Xeption
    {
        public FailedServiceTransferException(Exception innerException)
           : base(message: "Failed transfer service error occurred, please contact support.", innerException)
        { }
    }
}
