using drustvenaMreza.Model;
using drustvenaMreza.Repozitorijumi;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using System.Xml.Linq;

namespace drustvenaMreza.Controllers
{
    [Route("api/korisnici")]
    [ApiController]
    public class KorisniciController : ControllerBase
    {
        //[HttpGet]
        //public ActionResult<List<Korisnik>> GetAll()
        //{
        //    KorisniciRepozitorijum korisniciRepo = new KorisniciRepozitorijum();
        //    GrupeRepozitorijum grupeRepo = new GrupeRepozitorijum();

        //    List<Korisnik> korisnici = KorisniciRepozitorijum.Data.Values.ToList();
        //    return Ok(korisnici);
        //}
        //svi korisnici iz baze
        [HttpGet]
        public ActionResult<List<Korisnik>> GetAllFromDatabase()
        {
            try
            {
                using SqliteConnection connection = new SqliteConnection("Data Source=database/drustvenamreza.db");
                connection.Open();
                string query = "SELECT * FROM Users";
                using SqliteCommand command = new SqliteCommand(query, connection);
                using SqliteDataReader reader = command.ExecuteReader();

                List<Korisnik> results = new List<Korisnik>();
                while (reader.Read())
                {
                    //Username, Name, Surname, Birthday
                    Korisnik korisnik = new Korisnik
                    (
                        Convert.ToInt32(reader["Id"]),
                        reader["Username"].ToString(),
                        reader["Name"].ToString(),
                        reader["Surname"].ToString(),
                        DateTime.Parse(reader["Birthday"].ToString())
                        
                    );
                    results.Add(korisnik);
                }
                return Ok(results);

            }
            catch (SqliteException ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Greška pri radu sa bazom podataka.");
            }
            catch (FormatException ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Greška u formatu podataka iz baze.");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Neispravna operacija nad bazom podataka.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Došlo je do neočekivane greške.");
            }

        }

        [HttpGet("{id}")]
        public ActionResult<Korisnik> GetById(int id)
        {
            KorisniciRepozitorijum korisniciRepo = new KorisniciRepozitorijum();
            GrupeRepozitorijum grupeRepo = new GrupeRepozitorijum();

            if (!KorisniciRepozitorijum.Data.ContainsKey(id))
            {
                return NotFound();
            }
            return Ok(KorisniciRepozitorijum.Data[id]);
        }
        [HttpPost]
        public ActionResult<Korisnik> Create([FromBody] Korisnik noviKorisnik)
        {
            KorisniciRepozitorijum korisniciRepo = new KorisniciRepozitorijum();

            if (string.IsNullOrWhiteSpace(noviKorisnik.KorisnickoIme) || string.IsNullOrWhiteSpace(noviKorisnik.Ime) || string.IsNullOrWhiteSpace(noviKorisnik.Prezime))
            {
                return BadRequest();
            }

            noviKorisnik.Id = SracunajNoviId(KorisniciRepozitorijum.Data.Keys.ToList());
            KorisniciRepozitorijum.Data[noviKorisnik.Id] = noviKorisnik;
            korisniciRepo.Save();

            return Ok(noviKorisnik);
        }
        [HttpPut("{id}")]
        public ActionResult<Korisnik> Update(int id, [FromBody] Korisnik updatedKorisnik)
        {
            KorisniciRepozitorijum korisniciRepo = new KorisniciRepozitorijum();
            if (string.IsNullOrWhiteSpace(updatedKorisnik.KorisnickoIme) || string.IsNullOrWhiteSpace(updatedKorisnik.Ime) || string.IsNullOrWhiteSpace(updatedKorisnik.Prezime))
            {
                return BadRequest();
            }
            if (!KorisniciRepozitorijum.Data.ContainsKey(id))
            {
                return NotFound();
            }

            Korisnik korisnik = KorisniciRepozitorijum.Data[id];
            korisnik.KorisnickoIme = updatedKorisnik.KorisnickoIme;
            korisnik.Ime = updatedKorisnik.Ime;
            korisnik.Prezime = updatedKorisnik.Prezime;
            korisnik.DatumRodjenja = updatedKorisnik.DatumRodjenja;
            korisniciRepo.Save();

            return Ok(korisnik);
        }
        private static int SracunajNoviId(List<int> korisnici)
        {
            int max = 0;
            foreach (int id in korisnici)
            {
                if (max < id)
                {
                    max = id;
                }
            }
            return max+1;
        }

    }
}
