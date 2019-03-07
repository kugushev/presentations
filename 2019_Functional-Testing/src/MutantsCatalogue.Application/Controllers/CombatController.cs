using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MutantsCatalogue.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CombatController : ControllerBase
    {
        private const string Wolverine = "Wolverine";
        private const string Magneto = "Magneto";
        
        [HttpGet("result")]
        public ActionResult<string> GetCombatResult([FromQuery] string attacker, [FromQuery] string defender)
        {
            // todo: move all logic to domain
            var mutants = new[] {attacker, defender};
            if (mutants.Contains(Wolverine) && mutants.Contains(Magneto))
                return Magneto;

            if (mutants.Contains(Wolverine))
                return Wolverine;

            return "don't know";
        }

        [HttpPost("result/epic")]
        public ActionResult<string> PostEpicCombat([FromQuery] string attacker, [FromQuery] string defender)
        {
            throw new NotImplementedException();
        }
    }
}