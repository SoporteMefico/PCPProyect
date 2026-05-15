using System.ComponentModel.DataAnnotations.Schema;

namespace PCPProyect.Models
{
    public class Movdete1ot01
    {
        [Column("CODDOC")]
        public string CodDoc { get; set; }   // CODDOC
        [Column("NUMDOC")]
        public string NumDoc { get; set; }   // NUMDOC
        [Column("NUMITE")]
        public string NumIte { get; set; }   // NUMITE
        [Column("NUMITE1")]
        public string NumIte1 { get; set; }  // NUMITE1

        public DateTime? FecDoc { get; set; } // FECDOC
        public DateTime? FecEnt { get; set; } // FECENT
        [Column("CODSUBALM")]
        public string CodSubAlm { get; set; } // CODSUBALM
        [Column("CODART")]
        public string? CodArt { get; set; }    // CODART
        [Column("DESART")]
        public string? DesArt { get; set; }    // DESART
        [Column("XTIPUNI")]
        public string XTipUni { get; set; }   // XTIPUNI

        public decimal Peso { get; set; }     // PESO
        public decimal CanTot { get; set; }   // CANTOT
    }
}
