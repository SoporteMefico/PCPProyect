namespace PCPProyect.ViewModel
{
    public class PCPControlFiltroVM
    {
        public DateTime? FechaDesde { get; set; }

        public DateTime? FechaHasta { get; set; }

        public string Buscar { get; set; }

        public string EstadoOT { get; set; }

        public string EstadoPCP { get; set; }

        public bool Atrasado { get; set; }

        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}
