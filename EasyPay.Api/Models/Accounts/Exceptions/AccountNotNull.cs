//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using Xeptions;

namespace EasyPay.Api.Models.Accounts.Exceptions
{
    public class AccountNotNull : Xeption
    {
        public AccountNotNull()
            : base(message: "Account is null")
        { }
    }
}
