//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using EasyPay.Api.Models.Accounts;
using EasyPay.Api.Models.Accounts.Exceptions;
using EasyPay.Api.Models.Transfers.Exceptions;

namespace EasyPay.Api.Services.Processings
{
    public partial class TransferProcessingService
    {
        public void ValidateTransferOnAdd(Account sourceAccount, decimal amount)
        {
            ValidateTransferAmount(amount);

            ValidateAccountInsufficientFunds(sourceAccount, amount);
        }

        private static void ValidateAccountInsufficientFunds(Account sourceAccount, decimal amount)
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
