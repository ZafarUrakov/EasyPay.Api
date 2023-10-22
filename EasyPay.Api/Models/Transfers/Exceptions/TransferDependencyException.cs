//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using System;
using Xeptions;

namespace EasyPay.Api.Models.Transfers.Exceptions
{
    public class TransferDependencyException : Xeption
    {
        public TransferDependencyException(Exception innerException)
            : base(message: "Transafer dependency error occured, fix the error and try again.", innerException)
        { }
    }
}
