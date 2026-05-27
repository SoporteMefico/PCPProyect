using PCPProyect.ViewModel;

namespace PCPProyect.Models
{
    public class ProyeccionGridVM
    {
        public string CodDoc { get; set; }
        public string NumDoc { get; set; }

        public string Cliente { get; set; }
        public string Pedido { get; set; }

        public string NumIte { get; set; }

        public string CodArt { get; set; }
        public string DesArt { get; set; }

        public decimal Cantidad { get; set; }
        public decimal PesoUnitario { get; set; }
        public string Moneda { get; set; }
        public decimal PrecioVenta { get; set; }
        public DateTime FechaPedido { get; set; }
        public DateTime FechaEntrega { get; set; }
        public string Aleacion { get; set; }
        public string Prioridad { get; set; }
        public string EstadoOT { get; set; }
        public string EstadoPCP { get; set; }
        public int TotalProyectado { get; set; }
        public int SaldoPendiente { get; set; }
        public decimal KgPendiente { get; set; }
        public int TotalFabricado { get; set; }
        public decimal KgFabricado { get; set; }
        public int SaldoFabricar { get; set; }
        public decimal PorcentajeCumplimiento { get; set; }
        public decimal PorcenatajeProgramado { get; set; }
        public bool Atrasado { get; set; }



        // columnas dinámicas
        public Dictionary<string, SemanaDataVM> Semanas { get; set; }
    = new();
    }
}
