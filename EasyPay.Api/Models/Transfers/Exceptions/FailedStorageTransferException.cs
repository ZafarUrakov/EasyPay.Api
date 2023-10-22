//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using System;
using Xeptions;

namespace EasyPay.Api.Models.Transfers.Exceptions
{
    public class FailedStorageTransferException : Xeption
    {
        public FailedStorageTransferException(Exception innerException)
            : base(message: "Failed transfer storage error occired, fix the error and try again.", innerException)
        { }
    }
}
