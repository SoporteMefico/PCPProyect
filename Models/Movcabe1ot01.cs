using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PCPProyect.Models
{
    public class Movcabe1ot01
    {
        [Key]
        [Column(Order = 0)]
        public string NumDoc { get; set; }

        [Key]
        [Column(Order = 1)]
        public string CodDoc { get; set; }
        [Column("CODANE")]
        public string CodAne { get; set; }   // CODANE
        [Column("NOMANE")]
        public string? NomAne { get; set; }   // NOMANE
        [Column("FECDOC")]
        public DateTime FecDoc { get; set; } // FECDOC
        [Column("FECENT")]
        public DateTime FecEnt { get; set; } // FECENT
        [Column("XESTDOC")]
        public string XEstDoc { get; set; }  // XESTDOC


    }
}
