using drustvenaMreza.Model;

namespace drustvenaMreza.Repozitorijumi
{
    public class KorisniciRepozitorijum
    {
        private const string filePath = "data/korisnici.csv"; 
        public static Dictionary<int, Korisnik> Data;

        public KorisniciRepozitorijum()
        {
            if (Data == null)
            {
                Load();
            }
        }

        private void Load()
        {
            Data = new Dictionary<int, Korisnik>();
            string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                string[] attributes = line.Split(',');
                int id = int.Parse(attributes[0]);
                string korisnickoIme = attributes[1];
                string ime = attributes[2];
                string prezime = attributes[3];
                DateTime datumRodjenja = DateTime.Parse(attributes[4]);
                Korisnik korisnik = new Korisnik(id, korisnickoIme,ime, prezime,datumRodjenja);
                Data[id] = korisnik;
                
            }
        }

        public void Save()
        {
            List<string> lines = new List<string>();
            foreach (Korisnik korisnik in Data.Values)
            {
                
                lines.Add($"{korisnik.Id},{korisnik.KorisnickoIme},{korisnik.Ime},{korisnik.Prezime},{korisnik.DatumRodjenja.ToString("yyyy-MM-dd")}");
            }
            File.WriteAllLines(filePath, lines);
        }
    }
}
