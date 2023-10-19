//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using EasyPay.Api.Models.Clients;
using EasyPay.Api.Models.Clients.Exceptions;
using System;
using System.Reflection.Metadata;

namespace EasyPay.Api.Services.Foundations.Clients
{
    public partial class ClientService
    {
        private void ValidateClientOnAdd(Client client)
        {
            ValidateClientNotNull(client);

            Validate(
                (Rule: IsInvalid(client.ClientId), Parameter: nameof(Client.ClientId)),
                (Rule: IsInvalid(client.FirstName), Parameter: nameof(Client.FirstName)),
                (Rule: IsInvalid(client.LastName), Parameter: nameof(Client.LastName)),
                (Rule: IsInvalid(client.BirthDate), Parameter: nameof(Client.BirthDate)),
                (Rule: IsInvalid(client.Email), Parameter: nameof(Client.Email)),
                (Rule: IsInvalid(client.PhoneNumber), Parameter: nameof(Client.PhoneNumber)),
                (Rule: IsInvalid(client.Address), Parameter: nameof(Client.Address)));
        }

        private static dynamic IsInvalid(Guid clientId) => new
        {
            Condition = clientId == default,
            Message = "Id is required"
        };

        private static dynamic IsInvalid(string text) => new
        {
            Condition = String.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static dynamic IsInvalid(DateTimeOffset date) => new
        {
            Condition = date == default,
            Message = "Date is required"
        };

        private static void ValidateClientNotNull(Client client)
        {
            if (client == null)
            {
                throw new NullClientException();
            }
        }

        private static void ValidateClientId(Guid clientId) =>
            Validate((Rule: IsInvalid(clientId), Parameter: nameof(Client.ClientId)));

        private static void ValidateStorageClient(Client maybeClient, Guid clientId)
        {
            if(maybeClient is null)
            {
                throw new NotFoundClientException(clientId);
            }
        }

        private static void ValidateClientOnModify(Client client)
        {
            ValidateClientNotNull(client);

            Validate(
               (Rule: IsInvalid(client.ClientId), Parameter: nameof(Client.ClientId)),
               (Rule: IsInvalid(client.FirstName), Parameter: nameof(Client.FirstName)),
               (Rule: IsInvalid(client.LastName), Parameter: nameof(Client.LastName)),
               (Rule: IsInvalid(client.BirthDate), Parameter: nameof(Client.BirthDate)),
               (Rule: IsInvalid(client.Email), Parameter: nameof(Client.Email)),
               (Rule: IsInvalid(client.PhoneNumber), Parameter: nameof(Client.PhoneNumber)),
               (Rule: IsInvalid(client.Address), Parameter: nameof(Client.Address)));
        }

        private static void ValidateAgainstStorageClientOnModify(Client client, Client storageClient)
        {
            ValidateStorageClient(storageClient, client.ClientId);

            Validate(
                (Rule: IsInvalid(client.ClientId), Parameter: nameof(Client.ClientId)),
                (Rule: IsInvalid(client.FirstName), Parameter: nameof(Client.FirstName)),
                (Rule: IsInvalid(client.LastName), Parameter: nameof(Client.LastName)),
                (Rule: IsInvalid(client.BirthDate), Parameter: nameof(Client.BirthDate)),
                (Rule: IsInvalid(client.Email), Parameter: nameof(Client.Email)),
                (Rule: IsInvalid(client.PhoneNumber), Parameter: nameof(Client.PhoneNumber)),
                (Rule: IsInvalid(client.Address), Parameter: nameof(Client.Address)));
        }

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidClientException = new InvalidClientException();

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
