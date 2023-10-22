//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using Xeptions;

namespace EasyPay.Api.Models.Accounts.Exceptions
{
    public class InsufficientFundsException : Xeption
    {
        public InsufficientFundsException()
            : base(message: "Insufficient funds on balance.")
        { }
    }
}
