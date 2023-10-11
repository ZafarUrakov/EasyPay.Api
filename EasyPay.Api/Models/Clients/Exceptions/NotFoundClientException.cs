//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using System;
using Xeptions;

namespace EasyPay.Api.Models.Clients.Exceptions
{
    public class NotFoundClientException : Xeption
    {
        public NotFoundClientException(Guid clientId)
            : base(message: $"Couldn't find client with id {clientId}.")
        { }
    }
}
