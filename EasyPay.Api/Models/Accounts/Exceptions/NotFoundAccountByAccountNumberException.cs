//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using Xeptions;

namespace EasyPay.Api.Models.Accounts.Exceptions
{
    public class NotFoundAccountByAccountNumberException : Xeption
    {
        public NotFoundAccountByAccountNumberException(string accountNumber)
            : base(message: $"Account is not found with number: {accountNumber}")
        { }
    }
}
