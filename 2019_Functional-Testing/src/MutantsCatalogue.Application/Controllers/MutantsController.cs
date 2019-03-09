using Microsoft.AspNetCore.Mvc;
using MutantsCatalogue.Domain.Mutants;

namespace MutantsCatalogue.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MutantsController : ControllerBase
    {
        private readonly IMutantsService mutantsService;

        public MutantsController(IMutantsService mutantsService)
        {
            this.mutantsService = mutantsService;
        }
        
        [HttpGet("{name}")]
        public ActionResult<Mutant> Get(string name)
        {
            var mutant = mutantsService.Retrieve(name);
            if (mutant != null)
                return mutant;

            return NotFound();
        }

        [HttpPost()]
        public ActionResult Post([FromBody] Mutant mutant)
        {
            mutantsService.Add(mutant);
            return Ok();
        }
        
    }
}