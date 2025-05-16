using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ipsilon_1.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public required string Nombre { get; set; }
        public required string Contrasena { get; set; }
    }
    public class Paquete
    {
        public int Id { get; set; }

        public int Repartidor { get; set; }

        public string? NombreRepartidor { get; set; }

        public required string Codigo { get; set; }

        private int _estado;
        public int Estado
        {
            get => _estado;
            set
            {
                _estado = value;
                if (_estado == 1 && HorEnt == null)
                {
                    HorEnt = DateTime.Now;
                }
            }
        }

        public DateTime HorSal { get; set; }

        public DateTime? HorEnt { get; set; }

        public string EstadoDescripcion => Estado switch
        {
            0 => "En proceso de entrega",
            1 => "Entregado",
            2 => "Cancelado",
            3 => "No entregado",
            _ => throw new NotImplementedException()
        };

        public required string link { get; set; }


    }
}
