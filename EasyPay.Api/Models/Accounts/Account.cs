//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using EasyPay.Api.Models.Clients;
using System;

namespace EasyPay.Api.Models.Accounts
{
    public class Account
    {
        public Guid AccountId { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string AccountNumber { get; set; }
        public decimal Balance { get; set; }
        public Guid ClientId { get; set; }
        public Client Client { get; set; }
    }
}
