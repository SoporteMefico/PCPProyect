namespace PCPProyect.Models
{
    public class ProyeccionUpdateMasivoDto
    {
        public string CodDoc { get; set; }
        public string NumDoc { get; set; }
        public string NumIte { get; set; }

        public List<ProyeccionUpdateDto> Semanas { get; set; }
    }
}
