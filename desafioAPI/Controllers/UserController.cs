using desafioAPI.Context;
using desafioAPI.DTO;
using desafioAPI.Models;
using desafioAPI.Repositories;
using desafioAPI.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace desafioAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly UserService _service;


        public UserController(UserService service) => _service = service;



        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<string?>> Login(string email, string password)
        {
            try
            {

                var result = await _service.Login(email, password);

                return Ok(result);

            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Post(UserPostRequestBody userDTO)
        {

            try
            {
                User user = new()
                {
                    Name = userDTO.Name,
                    Email = userDTO.Email,
                    Password = userDTO.Password,
                };

                await _service.CreateAccount(user);

                return Created("Account Created Successfully", user);

            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ApplicationException ex)
            {
                return BadRequest("Server Error");
            }


        }


    }
}
