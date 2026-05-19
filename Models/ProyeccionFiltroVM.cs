namespace PCPProyect.Models
{
    public class ProyeccionFiltroVM
    {
        public DateTime FechaDesde { get; set; }
        public DateTime FechaHasta { get; set; }
        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 50;

        public string? Buscar { get; set; }
        public string TipCambio { get; set; } // opcional

        public string CodCliente { get; set; } // opcional
    }
}
