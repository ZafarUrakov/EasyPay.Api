//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using EasyPay.Api.Models.Clients;
using EasyPay.Api.Models.Clients.Exceptions;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Xeptions;

namespace EasyPay.Api.Services.Foundations.Clients
{
    public partial class ClientService
    {
        private delegate ValueTask<Client> ReturningClientFunction();

        private async ValueTask<Client> TryCatch(ReturningClientFunction returningClientFunction)
        {
            try
            {
                return await returningClientFunction();
            }
            catch (NullClientException nullClientException)
            {
                throw CreateAndLogValidationException(nullClientException);
            }
            catch (InvalidClientException invalidClientException)
            {
                throw CreateAndLogValidationException(invalidClientException);
            }
            catch (NotFoundClientException notFoundClientException)
            {
                throw CreateAndLogValidationException(notFoundClientException);
            }
            catch (SqlException sqlException)
            {
                var failedClientStrageException = new FailedClientStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedClientStrageException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsClientException = new AlreadyExistsClientException(duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistsClientException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedClientException = new LockedClientException(dbUpdateConcurrencyException);

                throw CreateAndLogDependencyValidationException(lockedClientException);
            }
            catch (DbUpdateException dbUpdateException)
            {
                var failedServiceStrageException = new FailedClientStorageException(dbUpdateException);

                throw CreateAndLogDependencyException(failedServiceStrageException);
            }
            catch (Exception exception)
            {
                var failedClientServiceException = new FailedClientServiceException(exception);

                throw CreateAndLogServiceException(failedClientServiceException);
            }
        }

        private ClientServiceException CreateAndLogServiceException(Xeption innerException)
        {
            var clientServiceException = new ClientServiceException(innerException);
            this.loggingBroker.LogError(clientServiceException);

            return clientServiceException;
        }

        private ClientDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var clientDependencyException = new ClientDependencyException(exception);
            this.loggingBroker.LogError(clientDependencyException);

            return clientDependencyException;
        }

        private ClientDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var clientDependencyValidationException = new ClientDependencyValidationException(exception);
            this.loggingBroker.LogError(clientDependencyValidationException);

            return clientDependencyValidationException;
        }

        private ClientDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var clientDependencyException = new ClientDependencyException(exception);
            this.loggingBroker.LogCritical(clientDependencyException);

            return clientDependencyException;
        }

        private ClientValidationException CreateAndLogValidationException(Xeption exception)
        {
            var clientValidationException = new ClientValidationException(exception);
            this.loggingBroker.LogError(clientValidationException);

            return clientValidationException;
        }
    }
}
