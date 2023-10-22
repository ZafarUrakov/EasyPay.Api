//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using Xeptions;

namespace EasyPay.Api.Models.Accounts.Exceptions
{
    public class InvalidAccountException : Xeption
    {
        public InvalidAccountException()
            : base(message: "Account is invalid.")
        { }
    }
}
