//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using System;
using Xeptions;

namespace EasyPay.Api.Models.Transfers.Exceptions
{
    public class AlreadyExistsTranasferExceptoion : Xeption
    {
        public AlreadyExistsTranasferExceptoion(Exception innerException)
            : base(message: "Transfe already exists.", innerException)
        { }
    }
}
