//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using EasyPay.Api.Models.Accounts;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EasyPay.Api.Services.Foundations.Accounts
{
    public interface IAccountService
    {
        ValueTask<Account> AddAccountAsync(Account account);
        IQueryable<Account> RetrieveAllAccounts();
        ValueTask<Account> RetrieveAccountByIdAsync(Guid accountId);
        ValueTask<Account> RemoveAccountByIdAsync(Guid accountId);
        ValueTask<Account> ModifyAccountAsync(Account account);
    }
}
