//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using Xeptions;

namespace EasyPay.Api.Models.Clients.Exceptions
{
    public class ClientDependencyValidationException : Xeption
    {
        public ClientDependencyValidationException(Xeption innerException)
           : base(message: "Client dependency validation error occurred, fix the errors and try again.",
                 innerException)
        { }
    }
}
