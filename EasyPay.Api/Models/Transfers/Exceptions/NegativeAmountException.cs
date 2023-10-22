//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using Xeptions;

namespace EasyPay.Api.Models.Transfers.Exceptions
{
    public class NegativeAmountException : Xeption
    {
        public NegativeAmountException()
            : base(message: "You can not enter negative amount.")
        { }
    }
}
