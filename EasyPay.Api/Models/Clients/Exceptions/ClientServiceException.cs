//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using System;
using Xeptions;

namespace EasyPay.Api.Models.Clients.Exceptions
{
    public class ClientServiceException : Xeption
    {
        public ClientServiceException(Exception innerException)
            : base(message: "Client service error occurred, contact support.", innerException)
        { }
    }
}
