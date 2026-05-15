using System.ComponentModel.DataAnnotations.Schema;

namespace PCPProyect.Models
{
    public class Articulo
    {
        [Column("CodEmp")]
        public string CodEmp { get; set; }
        [Column("CodSubAlm")]
        public string CodSubAlm { get; set; }
        [Column("xTipArt")]
        public string xTipArt { get; set; }
        [Column("xTipAlm")]
        public string xTipAlm { get; set; }
        [Column("CodArt")]
        public string CodArt { get; set; }
        [Column("DesArt")]
        public string DesArt { get; set; }
        [Column("PesArt")]
        public decimal PesArt { get; set; }
        [Column("FlKAct")]
        public bool FlKAct { get; set; }

    }
}
