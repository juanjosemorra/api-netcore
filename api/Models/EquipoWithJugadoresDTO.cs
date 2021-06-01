using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class EquipoWithJugadoresDTO
    {                
        public string nombre { get; set; }
        public ICollection<Jugador> jugadores { get; set; }
    }
}
