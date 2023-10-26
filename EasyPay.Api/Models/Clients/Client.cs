//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using EasyPay.Api.Models.Accounts;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace EasyPay.Api.Models.Clients
{
    public class Client
    {
        public Guid ClientId { get; set; }
        [JsonIgnore]
        public string AccountNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Login { get; set; }
        public DateTimeOffset BirthDate { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        [JsonIgnore]
        public List<Account> Accounts { get; set; }
    }
}
