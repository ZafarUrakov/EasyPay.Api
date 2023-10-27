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
using System.Threading.Tasks;
using Xeptions;

namespace EasyPay.Api.Services.Processings
{
    public partial class TransferProcessingService
    {
        private delegate ValueTask<decimal> ReturningAmountFunctions();
        private delegate ValueTask<Transfer> ReturningTransferFunctions();

        private async ValueTask<decimal> TryCatch(ReturningAmountFunctions returningAmountFunctions)
        {
            try
            {
                return await returningAmountFunctions();
            }
            catch (NotFoundAccountByAccountNumberException notFoundAccountByAccountNumberException)
            {
                throw CreateAndLogAccountValidationException(notFoundAccountByAccountNumberException);
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

                throw CreateAndLogAccountServiceException(failedAccountServiceException);
            }
        }

        private async ValueTask<Transfer> TryCatch(ReturningTransferFunctions returningTransferFunctions)
        {
            try
            {
                return await returningTransferFunctions();
            }
            catch (SqlException sqlException)
            {
                FailedStorageTransferException failedStorageTransferException =
                    new FailedStorageTransferException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedStorageTransferException);
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

        private TransferValidationException CreateAndLogTransferValidationException(Xeption exception)
        {
            var transferValidationException = new TransferValidationException(exception);

            this.loggingBroker.LogError(transferValidationException);

            return transferValidationException;
        }
        private AccountValidationException CreateAndLogAccountValidationException(Xeption exception)
        {
            var accountValidationException = new AccountValidationException(exception);

            this.loggingBroker.LogError(accountValidationException);

            return accountValidationException;
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

        private AccountServiceException CreateAndLogAccountServiceException(Xeption exception)
        {
            var accountServiceException =
                new AccountServiceException(exception);

            this.loggingBroker.LogError(accountServiceException);

            return accountServiceException;
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
