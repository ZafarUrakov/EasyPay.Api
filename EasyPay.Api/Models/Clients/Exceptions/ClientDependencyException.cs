//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using System;
using Xeptions;

namespace EasyPay.Api.Models.Clients.Exceptions
{
    public class ClientDependencyException : Xeption
    {
        public ClientDependencyException(Exception innerException)
            : base(message: "Client dependency error occurred, contact support.", innerException)
        { }
    }
}
