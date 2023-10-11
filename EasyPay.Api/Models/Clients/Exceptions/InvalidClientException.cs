//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using Xeptions;

namespace EasyPay.Api.Models.Clients.Exceptions
{
    public class InvalidClientException : Xeption
    {
        public InvalidClientException()
            : base(message: "Client is invalid.")
        { }
    }
}
