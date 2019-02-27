using System.Collections.Generic;

namespace FilterLibrary
{
    public interface IEjendom
    {
        int Postnummer { get; set; }
        string Adresse { get; set; }
        string Pris { get; set; }
        string Link { get; set; }
        int Areal { get; set; }
        int GrundAreal { get; set; }
        int Rum { get; set; }
        int Byggeår { get; set; }
        string Liggetid { get; set; }
        string Prisudvikling { get; set; }
        string KvadratmeterPris { get; set; }
        string Ejerudgifter { get; set; }
        string BoligydForbrugsafh { get; set; }
        string Foto { get; set; }
        int HashCode { get; set; }
    }
}