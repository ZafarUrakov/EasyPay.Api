//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using EasyPay.Api.Models.Accounts;
using EasyPay.Api.Models.Accounts.Exceptions;
using EasyPay.Api.Models.Transfers;
using EasyPay.Api.Models.Transfers.Exceptions;
using System;

namespace EasyPay.Api.Services.Foundations.Transfers
{
    public partial class TransferService
    {
        public void ValidateTransferOnAdd(Transfer transfer)
        {
            Validate(
                (Rule: IsInvalid(transfer.TransferId), Parameter: nameof(Transfer.TransferId)),
                (Rule: IsInvalid(transfer.SourceAccountNumber), Parameter: nameof(Transfer.SourceAccountNumber)),
                (Rule: IsInvalid(transfer.ReceiverAccountNumber), Parameter: nameof(Transfer.ReceiverAccountNumber)),
                (Rule: IsInvalid(transfer.Amount), Parameter: nameof(Transfer.Amount)));
        }

        public void ValidateTransferOnAddByDeposit(Transfer transfer)
        {
            Validate(
               (Rule: IsInvalid(transfer.SourceAccountNumber), Parameter: nameof(Transfer.SourceAccountNumber)),
               (Rule: IsInvalid(transfer.Amount), Parameter: nameof(Transfer.Amount)));
        }

        private static dynamic IsInvalid(Guid trnasferId) => new
        {
            Condition = trnasferId == default,
            Message = "Id is required"
        };

        private static dynamic IsInvalid(string text) => new
        {
            Condition = String.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static dynamic IsInvalid(decimal amount) => new
        {
            Condition = amount == default,
            Message = "Amount is required"
        };

        private static void ValidateAccountNotFoundForTransfer(Account sourceAccount, string sourceAccountNumber, Account recieverAccount, string reveiverAccountNumber)
        {
            if (sourceAccount == null)
            {
                throw new NotFoundAccountByAccountNumberException(sourceAccountNumber);
            }
            if (recieverAccount == null)
            {
                throw new NotFoundAccountByAccountNumberException(reveiverAccountNumber);
            }
        }

        private static void ValidateAccountNotFoundForTransfer(Account account)
        {
            if (account == null)
            {
                throw new NotFoundAccountByAccountNumberException(account.AccountNumber);
            }
        }

        private static void ValidateAccountInsufficientFunds(decimal amount, Account sourceAccount)
        {
            if (sourceAccount.Balance < amount)
            {
                throw new InsufficientFundsException();
            }
        }

        private static void ValidateTransferAmount(decimal amount)
        {
            if (amount < 0)
            {
                throw new NegativeAmountException();
            }
        }

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidClientException = new InvalidTransferException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidClientException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidClientException.ThrowIfContainsErrors();
        }
    }
}
