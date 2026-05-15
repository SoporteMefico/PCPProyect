namespace PCPProyect.Models
{
    public class ProyeccionUpdateDto
    {
        public string CodDoc { get; set; }
        public string NumDoc { get; set; }

        public string NumIte { get; set; }

        public int Anio { get; set; }
        public int Semana { get; set; }

        public decimal Cantidad { get; set; }
    }
}
