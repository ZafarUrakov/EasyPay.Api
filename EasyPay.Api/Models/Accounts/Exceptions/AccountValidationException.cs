//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using Xeptions;

namespace EasyPay.Api.Models.Accounts.Exceptions
{
    public class AccountValidationException : Xeption
    {
        public AccountValidationException(Xeption innerException)
            : base(message: "Account validation error occurred, fix the error and try again",
                  innerException)
        { }
    }
}
