//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using System;
using Xeptions;

namespace EasyPay.Api.Models.Accounts.Exceptions
{
    public class AlreadyExistsAccountException : Xeption
    {
        public AlreadyExistsAccountException(Exception innerException)
            : base(message: "Account already exists.",
                  innerException)
        { }
    }
}
