//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using System;
using Xeptions;

namespace EasyPay.Api.Models.Transfers.Exceptions
{
    public class NotFoundTransferException : Xeption
    {
        public NotFoundTransferException(Guid transferId)
            : base(message: "Transfer not found with id " + transferId)
        { }
    }
}
