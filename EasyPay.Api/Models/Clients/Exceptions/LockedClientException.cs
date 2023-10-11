//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using System;
using Xeptions;

namespace EasyPay.Api.Models.Clients.Exceptions
{
    public class LockedClientException : Xeption
    {
        public LockedClientException(Exception innerException)
            : base(message: "Client is locked, please try again.", innerException)
        { }
    }
}
