//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using System;
using EasyPay.Api.Models.Accounts;

namespace EasyPay.Api.Models.Transfers
{
    public class Transfer
    {
        public Guid TransferId { get; set; }
        public decimal Amount { get; set; }
        public Account Accounts { get; set; }
        public string ReceiverAccountNumber { get; set; }
    }
}
