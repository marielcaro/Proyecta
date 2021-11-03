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
    public class DisciplinaController: ControllerBase
    {
        private readonly IDisciplina _disciplinaRepository;
        private readonly ProyectaContext _context;

        public DisciplinaController(IDisciplina disciplinaRepository, ProyectaContext context)
        {
            _context = context;
            _disciplinaRepository = disciplinaRepository;
        }
        //GetAll 
        //DEVUELVE TODOS LOS DISCIPLINAS EN UN LISTADO
        [HttpGet] //Verbo de http GET 
        [Route(template: "disciplinas")]
        public IActionResult Get()
        {
            var disciplina = _context.Disciplinas.ToList();
            var disciplinaVM = new List<AltaDisciplinaViewModel>();
            foreach (var d in disciplina)
            {
                disciplinaVM.Add(new AltaDisciplinaViewModel
                {
                    Id = d.Id,
                    NombreDisciplina = d.NombreDisciplina
                });
            }
            return Ok(disciplinaVM.OrderBy(x=>x.Id).ToList());
        }

        //GetOne
        // DEVUELVE INFO DETALLADA DE UN DISCIPLINA EN PARTICULAR
        [HttpGet] //Verbo de http GET 
        [Route(template: "DetailedDisciplina")]
        public IActionResult Get(int id, string nombreDisciplina)
        {
            var disciplina = _context.Disciplinas.ToList();
            var disciplinaVM = new List<AltaDisciplinaViewModel>();

            if (id>0)
            {
                disciplina = disciplina.Where(x => x.Id == id).ToList();

            }
            if (nombreDisciplina != null)
            {
                disciplina = disciplina.Where(x => x.NombreDisciplina == nombreDisciplina).ToList();
            }
            foreach (var d in disciplina)
            {
                disciplinaVM.Add(new AltaDisciplinaViewModel
                {
                    Id = d.Id,
                    NombreDisciplina = d.NombreDisciplina
                });
            }

            if (!disciplina.Any()) return BadRequest(error: $"No hay disciplinas con este criterio");


            return Ok(disciplinaVM);
        }



        // TODO CREATE - POST
        // ALTA DE DISCIPLINA
        [HttpPost] //Verbo de http POST
        public IActionResult Post(AltaDisciplinaViewModel ad)
        {
            //Comprueba si el nombre de usuario no existe ya en la base de datos, ya que debe ser unico para cada uno
            var disciplina = _context.Disciplinas.ToList();
            disciplina = disciplina.Where(x => x.Id == ad.Id).ToList();

            //si no hay ningun usuario con ese nombre entonces procede a guardarlo en la BD
            if ((disciplina == null) || (disciplina.Count == 0))
            {

                Disciplina dbDisciplina = new Disciplina
                {
                  Id=ad.Id,
                  NombreDisciplina=ad.NombreDisciplina
                };

                _context.Disciplinas.Add(dbDisciplina);
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
        //ACTUALIZO DATOS DEL DISCIPLINA
        [HttpPut] //Verbo de http PUT
        public IActionResult Put(Disciplina disciplina)
        {
            var originalDisciplina = _disciplinaRepository.Get(disciplina.Id);
            if (originalDisciplina == null) return BadRequest(error: $"El disciplina {disciplina.Id} no existe");

            originalDisciplina.NombreDisciplina = disciplina.NombreDisciplina;

            _disciplinaRepository.Update(originalDisciplina);
            return Ok();
          
        }

        //// TODO DELETE
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            if (_context.Disciplinas.FirstOrDefault(x => x.Id == id) == null) return BadRequest(error: "La Disciplina enviada no existe.");

            var internalDisciplina = _context.Disciplinas.Find(id);
            _context.Disciplinas.Remove(internalDisciplina);
            _context.SaveChanges();

            return Ok();
        }

    }
}
