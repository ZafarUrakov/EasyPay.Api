//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using Xeptions;

namespace EasyPay.Api.Models.Accounts.Exceptions
{
    public class AccountDependencyValidationException : Xeption
    {
        public AccountDependencyValidationException(Xeption innerException)
            : base(message: "Account dependency validation error occurred, fix the error and try again",
                  innerException)
        { }
    }
}
