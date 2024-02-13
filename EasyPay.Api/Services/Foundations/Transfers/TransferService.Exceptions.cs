//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using EasyPay.Api.Models.Accounts.Exceptions;
using EasyPay.Api.Models.Transfers;
using EasyPay.Api.Models.Transfers.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xeptions;

namespace EasyPay.Api.Services.Foundations.Transfers
{
    public partial class TransferService
    {
        private delegate ValueTask<Transfer> ReturningTransferFunctions();
        private delegate IQueryable<Transfer> ReturningTransfersFunction();

        private async ValueTask<Transfer> TryCatch(ReturningTransferFunctions returningTransferFunctions)
        {
            try
            {
                return await returningTransferFunctions();
            }
            catch (NullTransferException nullTransferException)
            {
                throw CreateAndLogTransferValidationException(nullTransferException);
            }
            catch (InvalidTransferException invalidTransferException)
            {
                throw CreateAndLogTransferValidationException(invalidTransferException);
            }
            catch (NotFoundTransferException notFoundTransferException)
            {
                throw CreateAndLogTransferValidationException(notFoundTransferException);
            }
            catch (InsufficientFundsException insufficientFundsException)
            {
                throw CreateAndLogTransferValidationException(insufficientFundsException);
            }
            catch (NegativeAmountException negativeAmountException)
            {
                throw CreateAndLogTransferValidationException(negativeAmountException);
            }
            catch (SqlException sqlException)
            {
                var failedStorageAccountException =
                    new FailedStorageAccountException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedStorageAccountException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedAccountException
                    = new LockedAccountException(dbUpdateConcurrencyException);

                throw CreateAndLogDependencyValidationException(lockedAccountException);
            }
            catch (DbUpdateException dbUpdateException)
            {
                var failedStorageAccountException
                    = new FailedStorageAccountException(dbUpdateException);

                throw CreateAndLogDependencyException(failedStorageAccountException);
            }
            catch (Exception exception)
            {
                var failedAccountServiceException
                    = new FailedAccountServiceException(exception);

                throw CreateAndLogTransferServiceException(failedAccountServiceException);
            }
        }

        private IQueryable<Transfer> TryCatch(ReturningTransfersFunction returningTransfersFunction)
        {
            try
            {
                return returningTransfersFunction();
            }
            catch (SqlException sqlException)
            {
                var failedStorageTransferException =
                    new FailedStorageTransferException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedStorageTransferException);
            }
            catch (Exception exception)
            {
                var failedStorageTransferException =
                    new FailedStorageTransferException(exception);

                throw CreateAndLogTransferServiceException(failedStorageTransferException);
            }
        }

        private TransferValidationException CreateAndLogTransferValidationException(Xeption exception)
        {
            var transferValidationException = new TransferValidationException(exception);

            this.loggingBroker.LogError(transferValidationException);

            return transferValidationException;
        }

        private AccountDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var accountDependencyException
                = new AccountDependencyException(exception);

            this.loggingBroker.LogCritical(accountDependencyException);

            return accountDependencyException;
        }

        private AccountDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var accountDependencyValidationException
                = new AccountDependencyValidationException(exception);

            this.loggingBroker.LogError(accountDependencyValidationException);

            return accountDependencyValidationException;
        }

        private AccountDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var accountDependencyException
                = new AccountDependencyException(exception);

            this.loggingBroker.LogError(accountDependencyException);

            return accountDependencyException;
        }

        private TransferServiceException CreateAndLogTransferServiceException(Xeption exception)
        {
            var rtansferServiceException =
                new TransferServiceException(exception);

            this.loggingBroker.LogError(rtansferServiceException);

            return rtansferServiceException;
        }
    }
}
