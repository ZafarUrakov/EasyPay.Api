//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using Xeptions;

namespace EasyPay.Api.Models.Transfers.Exceptions
{
    public class NullTransferException : Xeption
    {
        public NullTransferException()
            : base(message: "Transfer is null.")
        { }
    }
}
