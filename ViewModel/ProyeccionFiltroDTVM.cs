namespace PCPProyect.ViewModel
{
    public class ProyeccionFiltroDTVM
    {
        public string FechaDesde { get; set; }

        public string FechaHasta { get; set; }

        public int Draw { get; set; }

        public int Start { get; set; }

        public int Length { get; set; }

        public SearchVM Search { get; set; }
        public decimal SaldoPP01Minimo { get; set; } = 0;
    }
}
