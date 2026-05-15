namespace PCPProyect.Models
{
    public class SemanaVM
    {
        public int Anio { get; set; }
        public int Semana { get; set; }

        public string Key => $"{Anio}-{Semana}";
    }
}
