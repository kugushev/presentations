using Microsoft.AspNetCore.Mvc;

namespace MutantsCatalogue.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MutantsController : ControllerBase
    {
        
        [HttpGet("{name}")]
        public IActionResult Get(string name)
        {
            return Ok(new
            {
                Name = name
            });
        }

        [HttpPut("{name}")]
        public ActionResult Put([FromBody] object mutant)
        {
            return Ok();
        }
        
    }
}