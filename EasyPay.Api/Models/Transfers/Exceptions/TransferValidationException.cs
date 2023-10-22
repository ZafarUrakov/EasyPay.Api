//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using Xeptions;

namespace EasyPay.Api.Models.Transfers.Exceptions
{
    public class TransferValidationException : Xeption
    {
        public TransferValidationException(Xeption innerException)
            : base(message: "Transfer is invalid, fix the error and try again.", innerException)
        { }
    }
}
