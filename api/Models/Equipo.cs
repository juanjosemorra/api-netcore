using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class Equipo
    {
        [Key]
        public int equipoId { get; set; }
        public string nombre { get; set; }
    }
}
