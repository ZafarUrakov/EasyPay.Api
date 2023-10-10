//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using Microsoft.AspNetCore.Mvc;

namespace EasyPay.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : Controller
    {
        [HttpGet]
        public string Get() =>
            "Welcome to the pursuit of money.";
    }
}
