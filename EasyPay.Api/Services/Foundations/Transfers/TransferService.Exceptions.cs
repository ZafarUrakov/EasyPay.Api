//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using EasyPay.Api.Brokers.Loggings;
using EasyPay.Api.Models.Accounts.Exceptions;
using EasyPay.Api.Models.Transfers.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Xeptions;

namespace EasyPay.Api.Services.Foundations.Transfers
{
    public partial class TransferService
    {
        private delegate ValueTask<decimal> ReturningAmountFunctions();

        private async ValueTask<decimal> TryCatch(ReturningAmountFunctions returningAmountFunctions)
        {
            try
            {
                return await returningAmountFunctions();
            }
            catch (NotFoundAccountByAccountNumberException notFoundAccountByAccountNumberException)
            {
                throw CreateAndLogValidationException(notFoundAccountByAccountNumberException);
            }
            catch(InsufficientFundsException insufficientFundsException)
            {
                throw CreateAndLogValidationException(insufficientFundsException);
            }
            catch(NegativeAmountException negativeAmountException)
            {
                throw CreateAndLogValidationException(negativeAmountException);
            }
            catch(SqlException sqlException)
            {
                var failedStorageAccountException =
                    new FailedStorageAccountException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedStorageAccountException);
            }
            catch(DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedAccountException
                    = new LockedAccountException(dbUpdateConcurrencyException);

                throw CreateAndLogDependencyValidationException(lockedAccountException);
            }
            catch(DbUpdateException dbUpdateException)
            {
                var failedStorageAccountException
                    = new FailedStorageAccountException(dbUpdateException);

                throw CreateAndLogDependencyException(failedStorageAccountException);
            }
            catch (Exception exception)
            {
                var failedAccountServiceException
                    = new FailedAccountServiceException(exception);

                throw CreateAndLogServiceException(failedAccountServiceException);
            }
        }

        private AccountValidationException CreateAndLogValidationException(Xeption exception)
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

        private AccountServiceException CreateAndLogServiceException(Xeption exception)
        {
            var accountServiceException =
                new AccountServiceException(exception);

            this.loggingBroker.LogError(accountServiceException);

            return accountServiceException;
        }
    }
}
