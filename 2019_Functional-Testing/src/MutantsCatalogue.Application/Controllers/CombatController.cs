using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MutantsCatalogue.Domain;
using MutantsCatalogue.Domain.Combat;

namespace MutantsCatalogue.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CombatController : ControllerBase
    {
        private readonly ICombatService combatService;

        public CombatController(ICombatService combatService)
        {
            this.combatService = combatService;
        }
        
        [HttpGet]
        public async Task<ActionResult<CombatResult>> GetCombatResult([FromQuery] string attacker, [FromQuery] string defender)
        {
            return await combatService.ExecuteCombatAsync(new[] {attacker, defender}, false);
        }

        [HttpPost("epic")]
        public async Task<ActionResult<CombatResult>> PostEpicCombat([FromQuery] string attacker, [FromQuery] string defender)
        {
            return await combatService.ExecuteCombatAsync(new[] {attacker, defender}, true);
        }
    }
}