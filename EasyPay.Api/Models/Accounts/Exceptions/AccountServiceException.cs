//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using Xeptions;

namespace EasyPay.Api.Models.Accounts.Exceptions
{
    public class AccountServiceException : Xeption
    {
        public AccountServiceException(Xeption innerException)
            : base(message: "Account service error occurred, contact support.",
                  innerException)
        { }
    }
}
