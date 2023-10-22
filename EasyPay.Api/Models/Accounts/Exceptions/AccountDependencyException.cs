//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using Xeptions;

namespace EasyPay.Api.Models.Accounts.Exceptions
{
    public class AccountDependencyException : Xeption
    {
        public AccountDependencyException(Xeption innerException)
            : base(message: "Account dependency error occurred, fix the error and try again.",
                  innerException)
        { }
    }
}
