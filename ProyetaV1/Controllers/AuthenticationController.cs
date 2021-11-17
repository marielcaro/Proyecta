﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProyetaV1.Context;
using Microsoft.AspNetCore.Http;
using ProyetaV1.Entities;
using ProyetaV1.Repositories;
using ProyetaV1.Interfaces;
using ProyetaV1.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity;


namespace ProyetaV1.Controllers
{
    [ApiController]
    [Route(template: "api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        //private readonly IUser _userRepository;
        //private readonly ProyectaContext _context;

        public AuthenticationController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
           // _context = context;
            //_userRepository = userRepository;
        }

        [HttpPost]
        [Route(template: "registro")]
        public async Task<IActionResult> Register(RegistroUsuarioViewModel model) //async para poder usar await
        {
            /**await para esperar a que el usuario termine*/
            //Revisar si existe usuario
            var userExists = await _userManager.FindByNameAsync(model.Username);

            //Si existe, devolver error
            if (userExists != null)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }

            //Si no existe, registrar al usuario
            var user = new User
            {
                UserName = model.Username,
                Email = model.Email,
                IsActive = true,
                UserType="Investigador"
            };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    status = "Error",
                    Message = $"User Creation Failed! Errors:{string.Join(", ", result.Errors.Select(x => x.Description))}"
                });

            }

            //await _mailService.SendMail(user);

            return Ok(new
            {
                status = "Success",
                Message = $"User created Succesfully!"


            }); ;
        }


        //Login
        [HttpPost]
        [Route(template: "login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            //Chequear que el usuario exista y que la password provista sea correcta
            var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, false, false);

            if (result.Succeeded)
            {
                var currentUser = await _userManager.FindByNameAsync(model.Username);
                if (currentUser.IsActive)
                {
                    //Generar el token
                    //Devolver Roken creado [y que no devuelva toda la info]
                    return Ok(await GetToken(currentUser));

                }
            }

            return StatusCode(StatusCodes.Status401Unauthorized, new
            {
                status = "Error",
                Message = $"User {model.Username} not authorized!"
            });



        }

        private async Task<LoginResponseViewModel> GetToken(User currentUser)
        {
            var userRoles = await _userManager.GetRolesAsync(currentUser);

            var authClaims = new List<Claim>()
            {
                new Claim (ClaimTypes.Name,currentUser.UserName),
                new Claim (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            //agregamos anuestra lista de privilegios o claims todos los privilegios de nuestro usuario
            authClaims.AddRange(userRoles.Select(x => new Claim(ClaimTypes.Role, x)));

            //levantamos nuestro signin key
            var authSignInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("KeySecretaSuperLargaDeAutorizacion")); //Encargado de proveernos info con la llave secreta para ingresar a la aplicación

            //creo el token
            var token = new JwtSecurityToken(
                issuer: "https://localhost:5001",
                audience: "https://localhost:5001",
                expires: DateTime.Now.AddHours(1),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSignInKey, SecurityAlgorithms.HmacSha256));

            return new LoginResponseViewModel
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                ValidTo = token.ValidTo
            };
        }




    }

}
