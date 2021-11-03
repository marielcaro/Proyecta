using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProyetaV1.Context;
using ProyetaV1.Entities;
using ProyetaV1.Repositories;
using ProyetaV1.Interfaces;
using ProyetaV1.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;


namespace ProyetaV1.Controllers
{
    [ApiController]
    [Route(template: "api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUser _userRepository;
        private readonly ProyectaContext _context;

        public UserController(IUser userRepository, ProyectaContext context)
        {
            _context = context;
            _userRepository = userRepository;
        }

        //GetAll 
        //DEVUELVE TODOS LOS USUARIOS EN UN LISTADO
        [HttpGet] //Verbo de http GET 
        [Route(template: "users")]
        public IActionResult Get()
        {

            return Ok();
        }

            

        //GetOne
        // DEVUELVE INFO DETALLADA DE UN USUARIO EN PARTICULAR
        [HttpGet] //Verbo de http GET 
        [Route(template: "DetailedUser")]
        public IActionResult Get(int id, string userName)
        {
            var user = _context.Users.Include(x=>x.Perfil).ToList();
            var perfilVM = new List<IdPerfilViewModel>();
            //Include(x=>x.Perfil)

            if (id >0)
            {
                user = user.Where(x => x.Id == id).ToList();
            }

            if (userName != null)
            {
                user = user.Where(x => x.UserName == userName).ToList();
            }

            if ( (user.Count > 0))
            {

                foreach (var p in user)
                {
                    perfilVM.Add(new IdPerfilViewModel
                    {
                      
                      Perfil=p.Perfil,
                      Email=p.Email
                    });
                }
                return Ok(perfilVM);
            }
            else
            {
                return BadRequest(error: "No hay ningun usuario con ese nombre/id");
            }
        }

        //GetOneSpecific
        // DEVUELVE VALIDACION DETALLADA DE UN USUARIO EN PARTICULAR
        //ESTE SE USARÍA PARA EL LOGIN
        [HttpGet] //Verbo de http GET 
        [Route(template: "ValidationUser")]
        public IActionResult Get(string userName, string password)
        {
            var user = _context.Users.ToList();
            user = user.Where(x => x.UserName == userName).ToList();

            //si no hay ningun usuario con ese nombre entonces procede a verificar contraseña
            if ((user.Count == 1))
            {
                var us = user.First();

                if (password != null)
                {
                    bool Valido = verificarPassword(password, us.Password, us.Token);

                    if (Valido)
                    {
                        return Ok(us.Token);
                    }
                    else
                    {
                        return BadRequest(error:"Contraseña Inválida");
                    }


                }
                else
                {
                    return BadRequest(error: "Debe colocar contraseña");

                }

            }
            else
            {
                return BadRequest("Usuario inválido");
            }
        }


        // TODO CREATE - POST
        // ALTA DE USUARIO
        [HttpPost] //Verbo de http POST
        public IActionResult Post(RegistroUsuarioViewModel ru)
        {
            //Comprueba si el nombre de usuario no existe ya en la base de datos, ya que debe ser unico para cada uno
            var user = _context.Users.Include(x=>x.Perfil).ToList();
            user = user.Where(x => x.UserName == ru.UserName).ToList();

            //si no hay ningun usuario con ese nombre entonces procede a guardarlo en la BD
            if ((user == null) || (user.Count==0))
            {

                string token = generarToken();

                string primerPass = generarPassword(ru.Password, token);

                User dbUser = new User
                {
                    UserName = ru.UserName,
                    Password = primerPass,
                    Email = ru.Email,
                    Token = token,
                    CreationDate = ru.CreationDate,
                     Perfil=ru.Perfil
                };

                _context.Users.Add(dbUser);
                _context.SaveChanges();

               
                return Ok();
            }
            else
            {
                //Ya hay un usuario con ese nombre
                return BadRequest();
            }
        }

        // TODO UPDATE - PUT / PATCH
        //ACTUALIZO DATOS DEL USUARIO - PARA QUE CAMBIE CONTRASEÑA Y EMAIL
        [HttpPut] //Verbo de http PUT
        public IActionResult Put(User user)
        {
            var originalUser = _userRepository.Get(user.Id);
            if (originalUser == null) return BadRequest(error: $"El user {user.Id} no existe");

            originalUser.Email = user.Email;
            originalUser.Password = user.Password;
            originalUser.Perfil = user.Perfil;

            _userRepository.Update(originalUser);

            return Ok();
           
        }

      

        //// TODO DELETE
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            if (_context.Users.FirstOrDefault(x => x.Id == id) == null) return BadRequest(error: "El user enviado no existe.");

            var internalUser = _context.Users.Find(id);
            _context.Users.Remove(internalUser);
            _context.SaveChanges();
            return Ok();
            
        }


        private string generarToken()
        {
            //a) Generate a random salt value:
            byte[] salt = new byte[32];
            System.Security.Cryptography.RNGCryptoServiceProvider.Create().GetBytes(salt);

            string Token = System.Text.Encoding.Default.GetString(salt);
            return Token;
        }

        private string generarPassword(string plainText, string token)
        {
            byte[] salt = System.Text.Encoding.ASCII.GetBytes(token);


            // b) Append the salt to the password.
            // Convert the plain string pwd into bytes
            byte[] plainTextBytes = System.Text.UnicodeEncoding.Unicode.GetBytes(plainText);
            // Append salt to pwd before hashing
            byte[] combinedBytes = new byte[plainTextBytes.Length + salt.Length];
            System.Buffer.BlockCopy(plainTextBytes, 0, combinedBytes, 0, plainTextBytes.Length);
            System.Buffer.BlockCopy(salt, 0, combinedBytes, plainTextBytes.Length, salt.Length);

            //c) Hash the combined password &salt:
            // Create hash for the pwd+salt
            System.Security.Cryptography.HashAlgorithm hashAlgo = new System.Security.Cryptography.SHA256Managed();
            byte[] hash = hashAlgo.ComputeHash(combinedBytes);

            //d) Append the salt to the resultant hash.
            // Append the salt to the hash
            byte[] hashPlusSalt = new byte[hash.Length + salt.Length];
            System.Buffer.BlockCopy(hash, 0, hashPlusSalt, 0, hash.Length);
            System.Buffer.BlockCopy(salt, 0, hashPlusSalt, hash.Length, salt.Length);

            //e) Store the result in your user store database.

            Console.WriteLine(System.Text.Encoding.Default.GetString(hashPlusSalt));
            return System.Text.Encoding.Default.GetString(hashPlusSalt);
        }

        private bool verificarPassword(string providedPass, string storedPass, string token)
        {
            byte[] salt = System.Text.Encoding.ASCII.GetBytes(token);

            // b) Append the salt to the password.
            // Convert the plain string pwd into bytes
            byte[] plainTextBytes = System.Text.UnicodeEncoding.Unicode.GetBytes(providedPass);
            // Append salt to pwd before hashing
            byte[] combinedBytes = new byte[plainTextBytes.Length + salt.Length];
            System.Buffer.BlockCopy(plainTextBytes, 0, combinedBytes, 0, plainTextBytes.Length);
            System.Buffer.BlockCopy(salt, 0, combinedBytes, plainTextBytes.Length, salt.Length);

            //c) Hash the combined password &salt:
            // Create hash for the pwd+salt
            System.Security.Cryptography.HashAlgorithm hashAlgo = new System.Security.Cryptography.SHA256Managed();
            byte[] hash = hashAlgo.ComputeHash(combinedBytes);

            //d) Append the salt to the resultant hash.
            // Append the salt to the hash
            byte[] hashPlusSalt = new byte[hash.Length + salt.Length];
            System.Buffer.BlockCopy(hash, 0, hashPlusSalt, 0, hash.Length);
            System.Buffer.BlockCopy(salt, 0, hashPlusSalt, hash.Length, salt.Length);

            //e) Store the result in your user store database.
            
            Console.WriteLine(System.Text.Encoding.Default.GetString(hashPlusSalt));

            if (System.Text.Encoding.Default.GetString(hashPlusSalt) == storedPass)
            {
                Console.WriteLine(true);
                return true;
            }
            Console.WriteLine(false);
            return false;

        }
    }

}
