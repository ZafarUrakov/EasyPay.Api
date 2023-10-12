//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using System;
using Xeptions;

namespace EasyPay.Api.Models.Clients.Exceptions
{
    public class AlreadyExistsClientException : Xeption
    {
        public AlreadyExistsClientException(Exception innerException)
            : base(message: "Client already exists.", innerException)
        { }
    }
}
