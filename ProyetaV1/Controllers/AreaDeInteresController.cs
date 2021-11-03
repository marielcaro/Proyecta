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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;


namespace ProyetaV1.Controllers
{
    [ApiController]
    [Route(template: "api/[controller]")]
    [Authorize]
    public class AreaDeInteresController: ControllerBase
    {
        private readonly IAreaDeIntereses _areaDeInteresesRepository;
        private readonly ProyectaContext _context;

        public AreaDeInteresController(IAreaDeIntereses areaDeInteresesRepository, ProyectaContext context)
        {
            _context = context;
            _areaDeInteresesRepository = areaDeInteresesRepository;
        }
        //GetAll 
        //DEVUELVE TODOS LAS AREAS DE INTERES EN UN LISTADO
        [HttpGet] //Verbo de http GET 
        [Route(template: "areas")]
        public IActionResult Get()
        {
            var area = _context.AreaDeIntereses.Include(x => x.Perfil).Include(y => y.Disciplina).ToList();
            var areaVM = new List<GetAreaDeInteres>();

            foreach (var d in area)
            {
                areaVM.Add(new GetAreaDeInteres
                {
                    Id = d.Id,
                    IdPerfil = d.Perfil.Id,
                    IdDisciplina = d.Disciplina.Id
                });
            }

            return Ok(areaVM);
        }

        //GetOne
        // DEVUELVE INFO DETALLADA DE UN AREA DE INTERES EN PARTICULAR
        [HttpGet] //Verbo de http GET 
        [Route(template: "DetailedArea")]
        public IActionResult Get(int id, int idPerfil, int idDisciplina)
        {
            var area = _context.AreaDeIntereses.Include(x=> x.Perfil).Include(y=>y.Disciplina).ToList();
            var areaVM = new List<GetAreaDeInteres>();

            if (id > 0)
            {
                area = area.Where(x => x.Id == id).ToList();


            }

            if (idPerfil > 0)
            {
                area = area.Where(x => x.Perfil.Id == idPerfil).ToList();
            }
            if (idDisciplina > 0)
            {
                area = area.Where(x => x.Disciplina.Id == idDisciplina).ToList();
            }

            foreach (var d in area)
            {
                areaVM.Add(new GetAreaDeInteres
                {
                    Id = d.Id,
                    IdPerfil = d.Perfil.Id,
                    IdDisciplina = d.Disciplina.Id
                });
            }

            if (!area.Any()) return BadRequest(error: $"No hay area de interes con este criterio");


            return Ok(areaVM);
 
        }


        // TODO CREATE - POST
        // ALTA DE AREA DE INTERES
        [HttpPost] //Verbo de http POST
        public IActionResult Post(AltaAreaDeInteres aa)
        {
            //Comprueba si el nombre de usuario no existe ya en la base de datos, ya que debe ser unico para cada uno
            var areaDeInteres = _context.AreaDeIntereses.Include(x=>x.Disciplina).Include(y=>y.Perfil).ToList();
            areaDeInteres = areaDeInteres.Where(x => x.Id == aa.Id).ToList();

            //si no hay ningun usuario con ese nombre entonces procede a guardarlo en la BD
            if ((areaDeInteres == null) || (areaDeInteres.Count == 0))
            {

                AreaDeInteres dbArea = new AreaDeInteres
                {
                    Id = aa.Id,
                    Disciplina = aa.Disciplina,
                    Perfil=aa.Perfil
                };

                _context.AreaDeIntereses.Add(dbArea);
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
        //ACTUALIZO DATOS DEL AREA DE INTERES
        [HttpPut] //Verbo de http PUT
        public IActionResult Put(AreaDeInteres areaDeInteres)
        {
            var originalAreaDeInteres = _areaDeInteresesRepository.Get(areaDeInteres.Id);
            if (originalAreaDeInteres == null) return BadRequest(error: $"El area {areaDeInteres.Id} no existe");

            originalAreaDeInteres.Disciplina = areaDeInteres.Disciplina;

            _areaDeInteresesRepository.Update(originalAreaDeInteres);
            return Ok();
       
        }

        //// TODO DELETE
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            if (_context.AreaDeIntereses.FirstOrDefault(x => x.Id == id) == null) return BadRequest(error: "La Area de Interes enviada no existe.");

            var internalArea = _context.AreaDeIntereses.Find(id);
            _context.AreaDeIntereses.Remove(internalArea);
            _context.SaveChanges();

            return Ok();
        }
    }
}
