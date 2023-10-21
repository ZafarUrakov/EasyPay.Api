//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using EasyPay.Api.Models.Accounts;
using EasyPay.Api.Models.Transfers;
using Microsoft.EntityFrameworkCore;

namespace EasyPay.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        DbSet<Transfer> Transfers { get; set; }

        //private readonly Account account;

        //public StorageBroker(Account account)
        //{
        //    this.account = account;
        //}

        public decimal GetBalance()
        {
            Transfer transfer = new Transfer();
            Account account = transfer.Accounts;

            return account.Balance;
        }
    }
}
