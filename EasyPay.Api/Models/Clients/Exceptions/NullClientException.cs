//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using Xeptions;

namespace EasyPay.Api.Models.Clients.Exceptions
{
    public class NullClientException : Xeption
    {
        public NullClientException()
            : base(message: "Client is null.")
        { }
    }
}
