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
            if (string.IsNullOrWhiteSpace(noviKorisnik.KorisnickoIme) || string.IsNullOrWhiteSpace(noviKorisnik.Ime) || string.IsNullOrWhiteSpace(noviKorisnik.Prezime))
            {
                return BadRequest();
            }
            Korisnik korisnik = userDbRepository.Create(noviKorisnik);
            if(korisnik == null)
            {
                return NotFound();
            }
            return Ok(korisnik);
        }
        [HttpPut("{id}")]
        public ActionResult<Korisnik> Update(int id, [FromBody] Korisnik updatedKorisnik)
        {
            
            if (string.IsNullOrWhiteSpace(updatedKorisnik.KorisnickoIme) || string.IsNullOrWhiteSpace(updatedKorisnik.Ime) || string.IsNullOrWhiteSpace(updatedKorisnik.Prezime))
            {
                return BadRequest();
            }

            Korisnik korisnik = userDbRepository.Update(id, updatedKorisnik);
            if (korisnik == null)
            {
                return NotFound();
            }

          

            return Ok(korisnik);
        }
        [HttpDelete("{id}")]
        public ActionResult<Korisnik> Delete(int id)
        {

            int korisnikObrisan = userDbRepository.Delete(id);
            if (korisnikObrisan == 0)
            {
                return NotFound();
            }

            return NoContent();
        }
        
    }
}
