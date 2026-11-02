using drustvenaMreza.Model;
using drustvenaMreza.Repozitorijumi;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Eventing.Reader;
using System.Text.RegularExpressions;

namespace drustvenaMreza.Controllers
{
    [Route("api/grupe")]
    [ApiController]
    public class GrupeController : ControllerBase
    {

       private KorisniciRepozitorijum korisniciRepozitorijum = new KorisniciRepozitorijum();
       private GrupeRepozitorijum grupeRepo = new GrupeRepozitorijum();

        [HttpGet]
        public ActionResult<List<Korisnik>> GetAll()
        {
            List<Grupa> grupa = GrupeRepozitorijum.Data.Values.ToList();
            return Ok(grupa);
        }
        [HttpGet("{id}/korisnici")]
        public ActionResult<List<Korisnik>> GetUsersByGroup(int id)
        {

            if (!GrupeRepozitorijum.Data.ContainsKey(id))
            {
                return NotFound();
            }

            Grupa grupa = GrupeRepozitorijum.Data[id];
            return Ok(grupa.Korisnici);
        }
    }
}
