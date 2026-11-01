using drustvenaMreza.Model;
using drustvenaMreza.Repozitorijumi;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Eventing.Reader;
using System.Text.RegularExpressions;

namespace drustvenaMreza.Controllers
{
    [Route("api/groups/{groupId}/users")]
    [ApiController]
    public class GrupeController : ControllerBase
    {

        private KorisniciRepozitorijum korisniciRepozitorijum = new KorisniciRepozitorijum();
       private GrupeRepozitorijum grupeRepo = new GrupeRepozitorijum();

        [HttpGet]
        public ActionResult<List<Korisnik>> GetUsersByGroup(int groupId)
        {

            if (!GrupeRepozitorijum.Data.ContainsKey(groupId))
            {
                return NotFound();
            }

            Grupa grupa = GrupeRepozitorijum.Data[groupId];
            return Ok(grupa.Korisnici);
        }
    }
}
