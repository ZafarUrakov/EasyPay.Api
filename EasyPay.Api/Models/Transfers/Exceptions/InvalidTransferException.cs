//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using Xeptions;

namespace EasyPay.Api.Models.Transfers.Exceptions
{
    public class InvalidTransferException : Xeption
    {
        public InvalidTransferException()
            : base(message: "Transfer is invalid.")
        { }
    }
}
