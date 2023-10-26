//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using EasyPay.Api.Models.Accounts;
using System;

namespace EasyPay.Api.Models.Transfers
{
    public class Transfer
    {
        public Guid TransferId { get; set; }
        public decimal Amount { get; set; }
        string Password {  get; set; }
        public Account Account { get; set; }
        public string SourceAccountNumber { get; set; }
        public string ReceiverAccountNumber { get; set; }
    }
}
