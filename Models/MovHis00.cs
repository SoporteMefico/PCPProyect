using System.ComponentModel.DataAnnotations.Schema;

namespace PCPProyect.Models
{
    public class MovHis00
    {
        public string CodEmp { get; set; }
        public string CodDoc { get; set; }
        public string NumDoc { get; set; }

        [Column("FECHIS")]
        public DateTime FechaHis { get; set; }

        public DateTime? FecIniPro { get; set; }

        public DateTime? FecDoc { get; set; }
        [Column("DESHIS")]
        public string? DesHis { get; set; }
        [Column("NUM1")]
        public decimal? CantidadProyectada { get; set; } // NUM1
        [Column("MOD0")]
        public string? Mod0 { get; set; }
        [Column("MOD1")]
        public string? Mod1 { get; set; }
        [Column("MOD3")]
        public string? Mod3 { get; set; }
        [Column("TIPEVE")]
        public string? TipEve { get; set; }
        [Column("NUMCOR")]
        public int NumCor { get; set; }
        [Column("ACTINC")]
        public bool ActInc { get; set; }
        [Column("DESEVE")]
        public string? DesEve { get; set; }
        [Column("ESTEVE")]
        public string? EstEve { get; set; }
        [Column("NUMITE")]
        public string? NumIte { get; set; }
        [Column("NUMITE1")]
        public string? NumIte1 { get; set; }

        public string? NomPc { get; set; }
    }
}
