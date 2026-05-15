namespace PCPProyect.Models
{
    public class VWProyeccionGrid
    {
        public string CodDoc { get; set; }

        public string NumDoc { get; set; }

        public string NumIte { get; set; }

        public string Cliente { get; set; }

        public string CodArt { get; set; }

        public string DesArt { get; set; }

        public decimal Cantot { get; set; }

        public decimal PesoUnitario { get; set; }

        public DateTime? FecIniPro { get; set; }

        public int Semana { get; set; }

        public int Anio { get; set; }

        public decimal CantidadProyectada { get; set; }
    }
}
