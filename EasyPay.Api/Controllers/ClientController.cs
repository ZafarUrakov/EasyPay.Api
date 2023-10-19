//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using EasyPay.Api.Models.Clients;
using EasyPay.Api.Models.Clients.Exceptions;
using EasyPay.Api.Services.Foundations.Clients;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;
using System.Threading.Tasks;

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
    }
}