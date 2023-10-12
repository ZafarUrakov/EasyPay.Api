//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using System;
using Xeptions;

namespace EasyPay.Api.Models.Accounts.Exceptions
{
    public class LockedAccountException : Xeption
    {
        public LockedAccountException(Exception innerException)
            : base(message: "Account is locked, try later",
                  innerException)
        { }
    }
}
