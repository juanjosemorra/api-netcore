using api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Controllers
{
    [Route("api/equipos/{equipoId}/[controller]")]
    [ApiController]
    public class JugadoresController : ControllerBase
    {
        private readonly DBContext _dbContext;

        public JugadoresController(DBContext dbContext)
        {
            this._dbContext = dbContext;
        }


        [HttpGet()]
        public IActionResult GetJugadores(int equipoId)
        {
            return Ok(this._dbContext.Jugadores.ToList().FindAll(p=> p.equipoId.Equals(equipoId)));
        }


        [HttpGet("{id}", Name = "GetJugadorForEquipo")]
        public IActionResult GetJugadorByEquipo(int equipoId, int id)
        {
            if (!this._dbContext.Equipos.ToList().Exists(p => p.equipoId.Equals(equipoId)))
            {
                return NotFound();
            }

            Jugador jugador = this._dbContext.Jugadores.ToList().Find(p => p.equipoId.Equals(equipoId) && p.jugadorId.Equals(id));
            if (jugador == null)
            {
                return NotFound();
            }


            return Ok(jugador);
        }


        [HttpPost]
        public IActionResult CreateJugadorForEquipo(int equipoId, [FromBody] Jugador jugador)
        {
            if (!this._dbContext.Equipos.ToList().Exists(p => p.equipoId.Equals(equipoId)))
            {
                return NotFound();
            }

            if (jugador == null)
            {
                return BadRequest();
            }

            
            
            int nextId = (this._dbContext.Jugadores.Max(p => p.jugadorId) + 1);
            jugador.equipoId = equipoId;
            jugador.jugadorId = nextId;
            this._dbContext.Jugadores.Add(jugador);
            this._dbContext.SaveChanges();


            return CreatedAtRoute("GetJugadorForEquipo", new { equipoId = jugador.equipoId, id = jugador.jugadorId }, jugador);
        }



        [HttpDelete("{id}")]
        public IActionResult Borrar(int equipoId, int id)
        {
            if (!this._dbContext.Equipos.ToList().Exists(p => p.equipoId.Equals(equipoId)))
            {
                return NotFound();
            }

            Jugador jugador = this._dbContext.Jugadores.ToList().Find(p => p.equipoId.Equals(equipoId) && p.jugadorId.Equals(id));
            if (jugador == null)
            {
                return NotFound();
            }

            this._dbContext.Jugadores.Remove(jugador);
            this._dbContext.SaveChanges();

            return NoContent();
        }


        [HttpPut("{id}")]
        public IActionResult Update(int equipoId, int id, [FromBody]JugadorDTO jugadorDTO)
        {

            if (!this._dbContext.Equipos.ToList().Exists(p => p.equipoId.Equals(equipoId)))
            {
                return NotFound();
            }

            Jugador jugador = this._dbContext.Jugadores.ToList().Find(p => p.equipoId.Equals(equipoId) && p.jugadorId.Equals(id));
            if (jugador == null)
            {
                return NotFound();
            }


            jugador.apellido = jugadorDTO.apellido;
            jugador.posicion = jugadorDTO.posicion;

            this._dbContext.Jugadores.Update(jugador);
            this._dbContext.SaveChanges();

            return NoContent();
            //porque no ok(jugador)
        }


        [HttpPatch("{id}")]
        public IActionResult UpdateParcial(int equipoId, int id, [FromBody] JsonPatchDocument<JugadorDTO> jugadorPatch)
        {

            if (!this._dbContext.Equipos.ToList().Exists(p => p.equipoId.Equals(equipoId)))
            {
                return NotFound();
            }

            Jugador jugador = this._dbContext.Jugadores.ToList().Find(p => p.equipoId.Equals(equipoId) && p.jugadorId.Equals(id));
            if (jugador == null)
            {
                return NotFound();
            }

            JugadorDTO jugadorDTO = new JugadorDTO()
            {
                apellido = jugador.apellido,
                posicion = jugador.posicion
            };


            jugadorPatch.ApplyTo(jugadorDTO);

            jugador.apellido = jugadorDTO.apellido;
            jugador.posicion = jugadorDTO.posicion;

            this._dbContext.Jugadores.Update(jugador);

            return NoContent();
        }

    }
}
 