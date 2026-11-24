using drustvenaMreza.Model;
using drustvenaMreza.Repozitorijumi;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace drustvenaMreza.Controllers
{
    [Route("api/posts")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private PostDbRepository postDbRepository;

        public  PostsController(IConfiguration configuration)
        {
            this.postDbRepository = new PostDbRepository(configuration);
        }

        [HttpGet]
        public ActionResult<List<Objava>> GetAll()
        {
          
            try
            {
                List<Objava> objave = postDbRepository.GetAll();
                return Ok(objave);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Problem("An error occurred while fetching users.");
            }
        }
        
    }
}
