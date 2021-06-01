using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class Jugador
    {
        [Key]
        public int jugadorId { get; set; }
        public string apellido { get; set; }
        public string posicion { get; set; }
        public int equipoId { get; set; }
    }
}

