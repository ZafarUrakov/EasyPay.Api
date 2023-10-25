//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using EasyPay.Api.Models.Clients;
using System;
using System.Text.Json.Serialization;

namespace EasyPay.Api.Models.Accounts
{
    public class Account
    {
        [JsonIgnore]
        public Guid AccountId { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string AccountNumber { get; set; }
        [JsonIgnore]
        public decimal Balance { get; set; }
        [JsonIgnore]
        public Guid ClientId { get; set; }
        public Client Client { get; set; }
    }
}
