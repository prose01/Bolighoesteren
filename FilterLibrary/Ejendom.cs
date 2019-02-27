using System.ComponentModel.DataAnnotations;

namespace FilterLibrary
{
    public class Ejendom : IEjendom
    {
        [Key]
        public int PropertyId { get; set; }
        public int Postnummer { get; set; }
        public string Adresse { get; set; }
        public string Pris { get; set; }
        public string Link { get; set; }
        public int Areal { get; set; }
        public int GrundAreal { get; set; }
        public int Rum { get; set; }
        public int Byggeår { get; set; }
        public string Liggetid { get; set; }
        public string Prisudvikling { get; set; }
        public string KvadratmeterPris { get; set; }
        public string Ejerudgifter { get; set; }
        public string BoligydForbrugsafh { get; set; }
        public string Foto { get; set; }
        public int HashCode { get; set; }

    }
}
