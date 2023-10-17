//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using System;
using Xeptions;

namespace EasyPay.Api.Models.Accounts.Exceptions
{
    public class NotFoundAccountException : Xeption
    {
        public NotFoundAccountException(Guid accountId)
            : base(message: $"Account is not found with id: {accountId}")
        { }
    }
}
