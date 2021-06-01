using api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EquiposController : ControllerBase
    {
        private readonly DBContext _dbContext;
        public EquiposController(DBContext dbContext)
        {
            this._dbContext = dbContext;
        }

        [HttpHead]
        [HttpGet]
        public IActionResult GetEquipos()
        {
            return Ok(this._dbContext.Equipos);
        }


        [HttpGet("{id}", Name = "GetByID")]    
        public IActionResult GetEquipo(int id)
        {
            Equipo equipo = this._dbContext.Equipos.ToList().Find(p => p.equipoId.Equals(id));
            if (equipo == null)
            {
                return NotFound();
            }

            return Ok(equipo);
        }


        public IActionResult NuevoEquipo([FromBody] Equipo equipo)
        {
            if (equipo == null)
            {
                return BadRequest();            
            }

            if (this._dbContext.Equipos.ToList().Exists(p => p.nombre.Equals(equipo.nombre))) 
            {
                return Conflict();
            }


            int nextId = (this._dbContext.Equipos.Max(p => p.equipoId) + 1);

            this._dbContext.Equipos.Add(equipo);
            this._dbContext.SaveChanges();

            return CreatedAtRoute("GetByID", new { id = equipo.equipoId }, equipo);
        }



        [HttpPost]
        //Crear una equipo con jugadores
        public IActionResult NuevoEquipoWithJugadores([FromBody] EquipoWithJugadoresDTO equipoWithJugadoresDTO)
        {
            if (equipoWithJugadoresDTO == null)
            {
                return BadRequest();
            }

            if (this._dbContext.Equipos.ToList().Exists(p => p.nombre.Equals(equipoWithJugadoresDTO.nombre)))
            {
                return Conflict();
            }


            int nextEquipoId = (this._dbContext.Equipos.Max(p => p.equipoId) + 1);
            Equipo equipo = new Equipo()
            {
                equipoId = nextEquipoId,
                nombre = equipoWithJugadoresDTO.nombre
            };
            this._dbContext.Equipos.Add(equipo);


            
            if (equipoWithJugadoresDTO.jugadores != null && equipoWithJugadoresDTO.jugadores.Count() > 0)
            {
                int cantidad = 1;
                List<Jugador> jugadores = new List<Jugador>();
                int nextJugadorId = (this._dbContext.Jugadores.Max(p => p.jugadorId) + cantidad);
                foreach (Jugador jugador in equipoWithJugadoresDTO.jugadores)
                {
                    jugador.equipoId = nextEquipoId;
                    jugador.jugadorId = (nextJugadorId + cantidad);
                    jugadores.Add(jugador);
                    cantidad += 1;                    
                }
                this._dbContext.Jugadores.AddRange(jugadores);
            }

            this._dbContext.SaveChanges();

            return CreatedAtRoute("GetByID", new { id = equipo.equipoId }, equipo);
        }


        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] EquipoDTO equipoDTO)
        {
            if (equipoDTO == null)
            {
                return BadRequest();
            }

            Equipo equipo =  this._dbContext.Equipos.ToList().Find(p=> p.equipoId.Equals(id));
            if (equipo == null)
            {
                return NotFound();
            }

            equipo.nombre = equipoDTO.nombre;
 
            this._dbContext.Equipos.Update(equipo);
            this._dbContext.SaveChanges();

            return NoContent();
        }



        [HttpPatch("{id}")]
        public IActionResult UpdateParcial(int id, [FromBody] JsonPatchDocument<EquipoDTO> equipoPatch)
        {
            if (equipoPatch == null)
            {
                return BadRequest();
            }

            Equipo equipo = this._dbContext.Equipos.ToList().Find(p => p.equipoId.Equals(id));
            if (equipo == null)
            {
                return NotFound();
            }

            EquipoDTO equipoDTO = new EquipoDTO();
            equipoPatch.ApplyTo(equipoDTO);

            equipo.nombre = equipoDTO.nombre;

            this._dbContext.Equipos.Update(equipo);
            this._dbContext.SaveChanges();

            return NoContent();
        }


        [HttpDelete("{idEquipo}")]
        public IActionResult Borrar(int idEquipo)
        {

            Equipo equipo = this._dbContext.Equipos.ToList().Find(p => p.equipoId.Equals(idEquipo));
            if (equipo == null)
            {
                return NotFound();
            }
        
            this._dbContext.Equipos.Remove(equipo);
            this._dbContext.SaveChanges();

            return Ok();
        }


    }
}
