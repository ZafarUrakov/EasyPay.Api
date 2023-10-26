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
            ValidateTRansferNotNull(transfer);

            Validate(
                (Rule: IsInvalid(transfer.TransferId), Parameter: nameof(Transfer.TransferId)),
                (Rule: IsInvalid(transfer.SourceAccountNumber), Parameter: nameof(Transfer.SourceAccountNumber)),
                (Rule: IsInvalid(transfer.ReceiverAccountNumber), Parameter: nameof(Transfer.ReceiverAccountNumber)),
                (Rule: IsInvalid(transfer.Amount), Parameter: nameof(Transfer.Amount)));
        }

        public void ValidateTransferOnModify(Transfer transfer)
        {
            ValidateTRansferNotNull(transfer);

            Validate(
                (Rule: IsInvalid(transfer.TransferId), Parameter: nameof(Transfer.TransferId)),
                (Rule: IsInvalid(transfer.SourceAccountNumber), Parameter: nameof(Transfer.SourceAccountNumber)),
                (Rule: IsInvalid(transfer.ReceiverAccountNumber), Parameter: nameof(Transfer.ReceiverAccountNumber)),
                (Rule: IsInvalid(transfer.Amount), Parameter: nameof(Transfer.Amount)));
        }

        private static void ValidateAndAgainstStorageTransferOnModify(Transfer maybeTransfer, Transfer transfer)
        {
            ValidateStorageTransfer(maybeTransfer, transfer.TransferId);

            Validate(
                (Rule: IsInvalid(maybeTransfer.TransferId), Parameter: nameof(Transfer.TransferId)),
                (Rule: IsInvalid(maybeTransfer.SourceAccountNumber), Parameter: nameof(Transfer.SourceAccountNumber)),
                (Rule: IsInvalid(maybeTransfer.ReceiverAccountNumber), Parameter: nameof(Transfer.ReceiverAccountNumber)),
                (Rule: IsInvalid(maybeTransfer.Amount), Parameter: nameof(Transfer.Amount)));
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

        private static void ValidateAccountNotFoundForTransfer(Account sourceAccount,
            string sourceAccountNumber, Account recieverAccount, string reveiverAccountNumber)
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

        private static void ValidateTransferId(Guid transferId) =>
            Validate((Rule: IsInvalid(transferId), Parameter: nameof(Transfer.TransferId)));

        private static void ValidateTRansferNotNull(Transfer transfer)
        {
            if (transfer is null)
            {
                throw new NullTransferException();
            }
        }

        private static void ValidateStorageTransfer(Transfer maybeTransfer, Guid transferId)
        {
            if (maybeTransfer is null)
            {
                throw new NotFoundTransferException(transferId);
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
