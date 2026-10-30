using drustvenaMreza.Model;
using System.Globalization;

namespace drustvenaMreza.Repozitorijumi
{
    public class GrupeRepozitorijum
    {
        private const string filePath = "data/grupe.csv";
        private const string veze = "data/clanstva.csv";
        public static Dictionary<int, Grupa> Data;

        public GrupeRepozitorijum()
        {
            if (Data == null)
            {
                Load();
            }
        }

        private void Load()
        {
            Data = new Dictionary<int, Grupa>();
            string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                string[] attributes = line.Split(',');
                int id = int.Parse(attributes[0]);
                string ime = attributes[1];
                DateTime datumOsnivanja = DateTime.ParseExact(attributes[2],"yyyy-MM-dd",CultureInfo.InvariantCulture);
                Grupa grupa = new Grupa(id, ime, datumOsnivanja);
                Data[id] = grupa;

                
            }
            string[] linijeVeza = File.ReadAllLines(veze);
            foreach (string line in linijeVeza)
            {
                string[] delovi = line.Split(",");
                int idKorisnika = int.Parse(delovi[0]);
                int idGrupe = int.Parse(delovi[1]);

             
                Data[idGrupe].Korisnici.Add(KorisniciRepozitorijum.Data[idKorisnika]);
            }
            
        }
        public void Save()
        {
            List<string> linije = new List<string>();
            List<string> linijeVeza = new List<string>();

            foreach (Grupa grupa in Data.Values)
            {
                linije.Add($"{grupa.Id},{grupa.Ime},{grupa.DatumOsnivanja.ToString("yyyy-MM-dd")}");
                foreach (Korisnik korisnik in grupa.Korisnici)
                {
                    linijeVeza.Add($"{korisnik.Id},{grupa.Id}");
                }
            }
            File.WriteAllLines(filePath, linije);
            File.WriteAllLines(veze, linijeVeza);

        }
    }
}
