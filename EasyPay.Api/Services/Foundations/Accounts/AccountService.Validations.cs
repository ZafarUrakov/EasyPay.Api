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
            ValidateAccountNotNull(account);

            Validate(
                (Rule: IsInvalid(account.AccountId), Parameter: nameof(Account.AccountId)),
                (Rule: IsInvalid(account.AccountNumber), Parameter: nameof(Account.AccountNumber)),
                (Rule: IsInvalid(account.Login), Parameter: nameof(Account.Login)),
                (Rule: IsInvalid(account.Password), Parameter: nameof(Account.Password)),
                (Rule: IsInvalid(account.ClientId), Parameter: nameof(Account.ClientId)));
        }

        private void ValidateAccountOnModify(Account account)
        {
            ValidateAccountNotNull(account);

            Validate(
                (Rule: IsInvalid(account.AccountId), Parameter: nameof(Account.AccountId)),
                (Rule: IsInvalid(account.AccountNumber), Parameter: nameof(Account.AccountNumber)),
                (Rule: IsInvalid(account.Login), Parameter: nameof(Account.Login)),
                (Rule: IsInvalid(account.Password), Parameter: nameof(Account.Password)),
                (Rule: IsInvalid(account.ClientId), Parameter: nameof(Account.ClientId)));
        }

        private void ValidateAgainstStorageAccountOnModify(Account inputAccount, Account storageAccount)
        {
            ValidateStorageAccount(storageAccount, inputAccount.AccountId);

            Validate(
                (Rule: IsInvalid(inputAccount.AccountId), Parameter: nameof(Account.AccountId)),
                (Rule: IsInvalid(inputAccount.AccountNumber), Parameter: nameof(Account.AccountNumber)),
                (Rule: IsInvalid(inputAccount.Login), Parameter: nameof(Account.Login)),
                (Rule: IsInvalid(inputAccount.Password), Parameter: nameof(Account.Password)),
                (Rule: IsInvalid(inputAccount.ClientId), Parameter: nameof(Account.ClientId)));
        }

        private static void ValidateAccountNotNull(Account account)
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

        private void ValidateAccountId(Guid accountId) =>
            Validate((Rule: IsInvalid(accountId), Parameter: nameof(Account.AccountId)));

        private void ValidateStorageAccount(Account maybeAccount, Guid accountId)
        {
            if (maybeAccount is null)
            {
                throw new NotFoundAccountException(accountId);
            }
        }

        private static void ValidateAccountNotFoundByAccountNumber(Account account, string accountNumber)
        {
            if (account == null)
            {
                throw new NotFoundAccountByAccountNumberException(accountNumber);
            }
        }

        private static void ValidateAccountNotFoundWithLoginOrAccountNumber(string login, string accountNumber, Account maybeAccount)
        {
            if (maybeAccount is null)
            {
                throw new NotFoundAccountWithLoginOrAccountNumber(login, accountNumber);
            }
        }

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
