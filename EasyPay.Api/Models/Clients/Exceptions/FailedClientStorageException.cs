//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using System;
using Xeptions;

namespace EasyPay.Api.Models.Clients.Exceptions
{
    public class FailedClientStorageException : Xeption
    {
        public FailedClientStorageException(Exception innerException)
            : base(message: "Failed client storage error occurred, contact support.", innerException)
        { }
    }
}
