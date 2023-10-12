//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using System.Threading.Tasks;
using EasyPay.Api.Models.Accounts;

namespace EasyPay.Api.Services.Foundations.Accounts
{
    public interface IAccountService
    {
        ValueTask<Account> AddAccountAsync(Account account);
    }
}
