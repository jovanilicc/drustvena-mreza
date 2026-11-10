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
        private UserDbRepository userDbRepository = new UserDbRepository();
        [HttpGet]
        public ActionResult<List<Korisnik>> GetAll()
        {
            List<Korisnik> korisnici = userDbRepository.GetAll();
            return Ok(korisnici);
        }
        

        [HttpGet("{id}")]
        public ActionResult<Korisnik> GetById(int id)
        {

            if (userDbRepository.GetById(id) == null)
            {
                return NotFound();
            }
            return Ok(userDbRepository.GetById(id));
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
