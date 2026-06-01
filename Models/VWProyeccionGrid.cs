using System.ComponentModel.DataAnnotations.Schema;

namespace PCPProyect.Models
{
    public class VWProyeccionGrid
    {
        public string Pedido { get; set; }
        public string CodDoc { get; set; }

        public string NumDoc { get; set; }

        public string NumIte { get; set; }

        public string Cliente { get; set; }

        public string CodArt { get; set; }

        public string DesArt { get; set; }
        [Column("Cantidad")]
        public decimal Cantot { get; set; }
        [Column("PesoUnitario")]
        public decimal PesoUnitario { get; set; }
        public decimal SaldoPP01 { get; set; }

    }
}
