//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using Xeptions;

namespace EasyPay.Api.Models.Clients.Exceptions
{
    public class ClientValidationException : Xeption
    {
        public ClientValidationException(Xeption innerException)
            : base(message: "Client validation error occurred, fix the errors and try again.", innerException)
        { }
    }
}
