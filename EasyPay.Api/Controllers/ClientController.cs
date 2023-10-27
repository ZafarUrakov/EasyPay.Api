//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using EasyPay.Api.Models.Clients;
using EasyPay.Api.Models.Clients.Exceptions;
using EasyPay.Api.Services.Processings.Clients;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EasyPay.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientController : RESTFulController
    {
        private readonly IClientProcessingService clientProcessingService;

        public ClientController(IClientProcessingService clientProcessingService)
        {
            this.clientProcessingService = clientProcessingService;
        }

        [HttpPost]
        public async ValueTask<ActionResult<string>> PostClientAsync(Client client)
        {
            try
            {
                var accountNumber = await this.clientProcessingService.RegisterClientWithAccountAsync(client);

                return Ok("Your accountNumber: " + accountNumber);
            }
            catch (ClientValidationException clientValidationException)
            {
                return BadRequest(clientValidationException.InnerException);
            }
            catch (ClientDependencyValidationException dependencyValidationException)
                when (dependencyValidationException.InnerException is AlreadyExistsClientException)
            {
                return Conflict(dependencyValidationException.InnerException);
            }
            catch (ClientDependencyException dependencyException)
            {
                return InternalServerError(dependencyException.InnerException);
            }
            catch (ClientServiceException serviceException)
            {
                return InternalServerError(serviceException.InnerException);
            }
        }

        [HttpGet("ById")]
        public async ValueTask<ActionResult<Client>> GetClientByIdAsync(Guid clientId)
        {
            try
            {
                return await this.clientProcessingService.RetrieveClientByIdAsync(clientId);
            }
            catch (ClientDependencyException dependencyException)
            {
                return InternalServerError(dependencyException.InnerException);
            }
            catch (ClientValidationException clientValidationException)
                when (clientValidationException.InnerException is InvalidClientException)
            {
                return BadRequest(clientValidationException.InnerException);
            }
            catch (ClientValidationException clientValidationException)
                when (clientValidationException.InnerException is NotFoundClientException)
            {
                return NotFound(clientValidationException.InnerException);
            }
            catch (ClientServiceException clientServiceException)
            {
                return InternalServerError(clientServiceException.InnerException);
            }
        }

        [HttpGet("All")]
        public ActionResult<IQueryable<Client>> GetAllClients()
        {
            try
            {
                IQueryable<Client> allClients = this.clientProcessingService.RetrieveAllClients();

                return Ok(allClients);
            }
            catch (ClientDependencyException clientDependencyException)
            {
                return InternalServerError(clientDependencyException.InnerException);
            }
            catch (ClientServiceException clientServiceException)
            {
                return InternalServerError(clientServiceException.InnerException);
            }
        }

        [HttpPut]
        public async ValueTask<ActionResult<Client>> PutClientAsync(Client client)
        {
            try
            {
                Client modifyClient =
                    await this.clientProcessingService.ModifyClientAsync(client);

                return Ok(modifyClient);
            }
            catch (ClientValidationException clientValidationException)
                when (clientValidationException.InnerException is NotFoundClientException)
            {
                return NotFound(clientValidationException.InnerException);
            }
            catch (ClientValidationException clientValidationException)
            {
                return BadRequest(clientValidationException.InnerException);
            }
            catch (ClientDependencyValidationException dependencyValidationException)
            {
                return Conflict(dependencyValidationException.InnerException);
            }
            catch (ClientDependencyException dependencyException)
            {
                return InternalServerError(dependencyException.InnerException);
            }
            catch (ClientServiceException serviceException)
            {
                return InternalServerError(serviceException.InnerException);
            }
        }

        [HttpDelete]
        public async ValueTask<ActionResult<Client>> DeleteAccountAsync(Guid clientId)
        {
            try
            {
                Client deleteClient = await this.clientProcessingService.RemoveClientByIdAsync(clientId);

                return Ok(deleteClient);
            }
            catch (ClientValidationException clientValidationException)
                when (clientValidationException.InnerException is NotFoundClientException)
            {
                return NotFound(clientValidationException.InnerException);
            }
            catch (ClientValidationException clientValidationException)
            {
                return BadRequest(clientValidationException.InnerException);
            }
            catch (ClientDependencyValidationException dependencyValidationException)
                when (dependencyValidationException.InnerException is LockedClientException)
            {
                return Locked(dependencyValidationException.InnerException);
            }
            catch (ClientDependencyValidationException dependencyValidationException)
            {
                return BadRequest(dependencyValidationException.InnerException);
            }
            catch (ClientDependencyException dependencyException)
            {
                return InternalServerError(dependencyException.InnerException);
            }
            catch (ClientServiceException serviceException)
            {
                return InternalServerError(serviceException.InnerException);
            }
        }
    }
}