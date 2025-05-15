namespace Ipsilon_1A.Models
{
    public class Paquete
    {
        public int Id { get; set; }
        public int Repártidor { get; set; }
        public required string Codigo { get; set; }
        public int Estado { get; set; } // "0 = En proceso de entrega", "1 = Entregado", "2 = Cancelado"

        public DateTime HorSal { get; set; }
        public DateTime? HorEnt { get; set; }

        public required string Link { get; set; }
    }
}
