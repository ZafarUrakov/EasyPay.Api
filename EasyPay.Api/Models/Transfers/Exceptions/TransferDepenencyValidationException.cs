//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using Xeptions;

namespace EasyPay.Api.Models.Transfers.Exceptions
{
    public class TransferDepenencyValidationException : Xeption
    {
        public TransferDepenencyValidationException(Xeption innerException)
            : base(message: "Transfer ependency validation error occurred, fix the errors and try again.", 
                  innerException)
        { }
    }
}
