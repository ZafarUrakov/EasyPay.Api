//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using Xeptions;

namespace EasyPay.Api.Models.Transfers.Exceptions
{
    public class LockedTransferException : Xeption
    {
        public LockedTransferException(Xeption innerException)
            : base(message: "Transfer is locked, please try again.", innerException)
        { }
    }
}
