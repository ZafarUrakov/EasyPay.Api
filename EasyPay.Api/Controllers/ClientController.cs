//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using EasyPay.Api.Models.Clients;
using EasyPay.Api.Models.Clients.Exceptions;
using EasyPay.Api.Services.Foundations.Clients;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Writers;
using RESTFulSense.Controllers;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xeptions;

namespace EasyPay.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientController : RESTFulController
    {
        private readonly IClientService clientService;

        public ClientController(IClientService clientService) =>
            this.clientService = clientService;

        [HttpPost]
        public async ValueTask<ActionResult<Client>> PostClientAsync(Client client)
        {
            try
            {
                return await this.clientService.AddClientAsync(client);
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

        [HttpGet("clientId")]
        public async ValueTask<ActionResult<Client>> GetClientByIdAsync(Guid clientId)
        {
            try
            {
                return await this.clientService.RetrieveClientByIdAsync(clientId);
            }
            catch (ClientDependencyException dependencyException)
            {
                return InternalServerError(dependencyException.InnerException);
            }
            catch (ClientValidationException clientValidationException)
                when(clientValidationException.InnerException is InvalidClientException)
            {
                return BadRequest(clientValidationException.InnerException);
            }
            catch (ClientValidationException clientValidationException)
                when(clientValidationException.InnerException is NotFoundClientException)
            {
                return NotFound(clientValidationException.InnerException);
            }
            catch (ClientServiceException clientServiceException)
            {
                return InternalServerError(clientServiceException.InnerException);
            }
        }
    }
}