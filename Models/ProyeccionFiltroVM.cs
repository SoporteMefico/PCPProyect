namespace PCPProyect.Models
{
    public class ProyeccionFiltroVM
    {
        public DateTime FechaDesde { get; set; }
        public DateTime FechaHasta { get; set; }
        public string TipCambio { get; set; } // opcional

        public string CodCliente { get; set; } // opcional
    }
}
