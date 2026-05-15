using PCPProyect.ViewModel;

namespace PCPProyect.Models
{
    public class ProyeccionGridVM
    {
        public string CodDoc { get; set; }
        public string NumDoc { get; set; }

        public string Cliente { get; set; }

        public string NumIte { get; set; }

        public string CodArt { get; set; }
        public string DesArt { get; set; }

        public decimal Cantidad { get; set; }
        public decimal PesoUnitario { get; set; }

        // columnas dinámicas
        public Dictionary<string, SemanaDataVM> Semanas { get; set; }
    = new();
    }
}
