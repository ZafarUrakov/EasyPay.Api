//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using Xeptions;

namespace EasyPay.Api.Models.Accounts.Exceptions
{
    public class NotFoundAccountWithLoginOrAccountNumber : Xeption
    {
        public NotFoundAccountWithLoginOrAccountNumber(string login, string accountNumber)
            : base(message: $"Account not found with login: {login} or account number: {accountNumber}")
        { }
    }
}
