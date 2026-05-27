namespace PCPProyect.ViewModel
{
    public class PCPControlGridVM
    {
        public string CodDoc { get; set; }
        public string NumDoc { get; set; }
        public DateTime? FecDoc { get; set; }

        public string Cliente { get; set; }

        public string Pedido { get; set; }

        public string CodArt { get; set; }
        public string DesArt { get; set; }

        public decimal Cantidad { get; set; }

        public decimal PesoUnitario { get; set; }

        public string Moneda { get; set; }

        public decimal PrecioVenta { get; set; }

        public DateTime? FechaPedido { get; set; }

        public DateTime? FechaEntrega { get; set; }

        public string SemanaEntrega { get; set; }

        public string Aleacion { get; set; }

        public string Prioridad { get; set; }

        public string EstadoOT { get; set; }

        public string EstadoPCP { get; set; }

        public decimal TotalProyectado { get; set; }

        public decimal SaldoPendiente { get; set; }

        public decimal KgPendiente { get; set; }

        public decimal TotalFabricado { get; set; }

        public decimal KgFabricado { get; set; }

        public decimal SaldoFabricar { get; set; }

        public decimal TotalPP01 { get; set; }

        public string EstadoAlmacen { get; set; }

        public decimal PendienteIngreso { get; set; }

        public decimal PorcentajeCumplimiento { get; set; }

        public decimal PorcentajeProgramado { get; set; }

        public bool Atrasado { get; set; }

        public decimal EficienciaPCP { get; set; }
    }
}
