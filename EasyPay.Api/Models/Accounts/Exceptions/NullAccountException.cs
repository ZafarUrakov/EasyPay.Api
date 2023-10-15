//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using Xeptions;

namespace EasyPay.Api.Models.Accounts.Exceptions
{
    public class NullAccountException : Xeption
    {
        public NullAccountException()
            : base(message: "Account is null")
        { }
    }
}
