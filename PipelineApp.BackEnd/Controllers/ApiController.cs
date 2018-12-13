﻿namespace PipelineApp.BackEnd.Controllers
{
    using System.Linq;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Route("api")]
    public class ApiController : Controller
    {
        [HttpGet]
        [Route("public")]
        public IActionResult Public()
        {
            return Json(new
            {
                Message = "Hello from a public endpoint! You don't need to be authenticated to see this."
            });
        }

        [HttpGet]
        [Route("private")]
        [Authorize]
        public IActionResult Private()
        {
            return Json(User.Claims.Select(c =>
                new
                {
                    c.Type,
                    c.Value
                }));
        }

        /// <summary>
        /// This is a helper action. It allows you to easily view all the claims of the token
        /// </summary>
        /// <returns></returns>
        [HttpGet("claims")]
        public IActionResult Claims()
        {
            return Json(User.Claims.Select(c =>
                new
                {
                    c.Type,
                    c.Value
                }));
        }
    }
}
