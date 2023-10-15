//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using EasyPay.Api.Models.Accounts;
using EasyPay.Api.Models.Accounts.Exceptions;
using System;

namespace EasyPay.Api.Services.Foundations.Accounts
{
    public partial class AccountService
    {
        private void ValidateAccountOnAdd(Account account)
        {
            ValidateClientNotNull(account);

            Validate(
                (Rule: IsInvalid(account.AccountId), Parameter: nameof(Account.AccountId)),
                (Rule: IsInvalid(account.AccountNumber), Parameter: nameof(Account.AccountNumber)),
                (Rule: IsInvalid(account.Login), Parameter: nameof(Account.Login)),
                (Rule: IsInvalid(account.Password), Parameter: nameof(Account.Password)),
                (Rule: IsInvalid(account.ClientId), Parameter: nameof(Account.ClientId)));
        }

        private static void ValidateClientNotNull(Account account)
        {
            if (account == null)
            {
                throw new NullAccountException();
            }
        }

        private dynamic IsInvalid(string text) => new
        {
            Condition = string.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private dynamic IsInvalid(Guid id) => new
        {
            Condition = id == default,
            Message = "Id is required"
        };

        private dynamic IsInvalid(int id) => new
        {
            Condition = id == default,
            Message = "Id is required"
        };

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidAccountException = new InvalidAccountException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidAccountException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidAccountException.ThrowIfContainsErrors();
        }
    }
}
