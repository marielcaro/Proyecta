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
    [Authorize]
    public class PerfilController: ControllerBase
    {
        private readonly IPerfil _perfilRepository;
        private readonly ProyectaContext _context;

        public PerfilController(IPerfil perfilRepository, ProyectaContext context)
        {
            _context = context;
            _perfilRepository = perfilRepository;
        }

        //GetAll 
        //DEVUELVE TODOS LOS PERFILES EN UN LISTADO
        [HttpGet] //Verbo de http GET 
        [Route(template: "perfiles")]
        public IActionResult Get()
        {
            return Ok();
        }

        //GetOne
        // DEVUELVE INFO DETALLADA DE UN PERFIL EN PARTICULAR
        [HttpGet] //Verbo de http GET 
        [Route(template: "DetailedPerfil")]
        public IActionResult Get(int id, string dni, string nombre)
        {
            var perfiles = _context.Perfiles.ToList();
            var perfilVM = new List<AltaPerfilViewModel>();

            if (id > 0)
            {
                perfiles = perfiles.Where(x => x.Id == id).ToList();

            }
            if (dni != null)
            {
                perfiles = perfiles.Where(x => x.DNI == dni).ToList();
            }
            if (nombre != null)
            {
                perfiles = perfiles.Where(x => x.NombreCompleto == nombre).ToList();
            }

            foreach (var p in perfiles)
            {
                perfilVM.Add(new AltaPerfilViewModel
                {
                    Id = p.Id,
                    NombreCompleto = p.NombreCompleto,
                    DNI = p.DNI,
                    FechaNacimiento = p.FechaNacimiento,
                    CarreraProfesional = p.CarreraProfesional,
                    Facultad = p.Facultad,
                    Telefono = p.Telefono
                });
            }

            if (!perfiles.Any()) return BadRequest(error: $"No hay perfiles con este criterio");


            return Ok(perfilVM);

        }

        //GetOneSpecific
        // FILTRO PARA BUSQUEDA DE PERFILES EN ESPECÍFICO
        //ESTE SE USARÍA PARA EL LOGIN
        [HttpGet] //Verbo de http GET 
        [Route(template: "FiltroPerfil")]
        
        public IActionResult Get(string universidad, string facultad, string carrera, string disciplina)
        {
            var perfiles = _context.Perfiles.Include(x=>x.AreaDeInteres).ThenInclude(y=>y.Disciplina).ToList();
            var perfilVM = new List<AltaPerfilViewModel>();

            if (universidad != null)
            {
                perfiles = perfiles.Where(x => x.Universidad == universidad).ToList();

            }
            if (facultad != null)
            {
                perfiles = perfiles.Where(x => x.Universidad == universidad).ToList();
            }
            if (carrera != null)
            {
                perfiles = perfiles.Where(x => x.CarreraProfesional == carrera).ToList();
            }

            if (disciplina != null)
            {
                perfiles = perfiles.Where(x => x.AreaDeInteres.Select(y => y.Disciplina.NombreDisciplina).Equals(disciplina)).ToList();


            }


            foreach (var p in perfiles)
            {
                perfilVM.Add(new AltaPerfilViewModel
                {
                    Id = p.Id,
                    NombreCompleto = p.NombreCompleto,
                    DNI = p.DNI,
                    FechaNacimiento = p.FechaNacimiento,
                    CarreraProfesional = p.CarreraProfesional,
                    Facultad = p.Facultad,
                    Telefono = p.Telefono
                });
            }

            if (!perfiles.Any()) return BadRequest(error: $"No hay perfiles con este criterio");


            return Ok(perfilVM);
        }


        // TODO CREATE - POST
        // ALTA DE PERFIL
        [HttpPost] //Verbo de http POST
        public IActionResult Post(AltaPerfilViewModel ap)
        {
            //Comprueba si el nombre de usuario no existe ya en la base de datos, ya que debe ser unico para cada uno
            var perfil = _context.Perfiles.ToList();
            perfil = perfil.Where(x => x.Id == ap.Id).ToList();

            //si no hay ningun usuario con ese nombre entonces procede a guardarlo en la BD
            if ((perfil == null) || (perfil.Count == 0))
            {

                Perfil dbPerfil = new Perfil
                {
                    DNI=ap.DNI,
                    NombreCompleto = ap.NombreCompleto,
                    FechaNacimiento = ap.FechaNacimiento,
                    CarreraProfesional = ap.CarreraProfesional,
                    Telefono = ap.Telefono,
                    Universidad = ap.Universidad,
                    Facultad=ap.Facultad,
                    FotoPerfil=ap.FotoPerfil
                };

                _context.Perfiles.Add(dbPerfil);
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
        //ACTUALIZO DATOS DEL PERFIL
        [HttpPut] //Verbo de http PUT
        public IActionResult Put(Perfil perfil)
        {
            var originalPerfil = _perfilRepository.Get(perfil.Id);
            if (originalPerfil == null) return BadRequest(error: $"El user {perfil.Id} no existe");

            originalPerfil.NombreCompleto = perfil.NombreCompleto;
            originalPerfil.Telefono = perfil.Telefono;
            originalPerfil.FotoPerfil = perfil.FotoPerfil;
            originalPerfil.Universidad = perfil.Universidad;
            originalPerfil.Facultad = perfil.Facultad;

            _perfilRepository.Update(originalPerfil);
            return Ok();
        }

        //// TODO DELETE
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            if (_context.Perfiles.FirstOrDefault(x => x.Id == id) == null) return BadRequest(error: "El perfil enviado no existe.");

            var internalPerfil = _context.Perfiles.Find(id);
            _context.Perfiles.Remove(internalPerfil);
            _context.SaveChanges();
       
            return Ok();
        }
    }
}
