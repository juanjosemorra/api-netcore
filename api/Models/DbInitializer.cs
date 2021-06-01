using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public static class DbInitializer
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var _context = new DBContext(serviceProvider.GetRequiredService<DbContextOptions<DBContext>>()))
            {
                
                if (_context.Equipos.Any())
                {
                    return;
                }

                _context.Equipos.AddRange(
                    new Equipo { equipoId = 1, nombre = "Manchester City" },
                    new Equipo { equipoId = 2, nombre = "Barcelona" },
                    new Equipo { equipoId = 3, nombre = "PSG" }
                 );



                _context.Jugadores.AddRange(
                    new Jugador { jugadorId = 1, apellido = "Aguero", posicion = "Delantero", equipoId = 1 },
                    new Jugador { jugadorId = 2, apellido = "Fernandinho", posicion = "Centro Campista", equipoId = 1 },
                    new Jugador { jugadorId = 3, apellido = "Pique", posicion = "Defensor", equipoId = 2 },
                    new Jugador { jugadorId = 4, apellido = "Messi", posicion = "Delantero", equipoId = 2 },
                    new Jugador { jugadorId = 5, apellido = "Neymar", posicion = "Delantero", equipoId = 3 },
                    new Jugador { jugadorId = 6, apellido = "Di Maria", posicion = "MediaPunta", equipoId = 3 },
                    new Jugador { jugadorId = 7, apellido = "Keylor", posicion = "Arquero", equipoId = 3 }
                 );


                _context.SaveChanges();
            }
        }
    }
}
 