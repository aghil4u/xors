using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Models;



namespace Server.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class VerificationsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public VerificationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Verifications
        [HttpGet]
        public IEnumerable<Verification> GetVerification()
        {
            return _context.Verification;
        }


        [HttpGet("timestamp/{TimeStamp}")]
        public IEnumerable<Verification> GetVerificationAsync([FromRoute] long timestamp)
        {

            var verifications = _context.Verification.Where(e => e.TimeStamp >= timestamp);

            return verifications;

        }

        // GET: api/Verifications/5

        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetVerification([FromRoute] int id)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var verification = await _context.Verification.FindAsync(id);

        //    if (verification == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(verification);
        //}

        [HttpGet("{id}")]
        public IActionResult GetVerification([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            List<Verification> verifications = _context.Verification.Where((p) => p.AssetNumber == id).ToList();

            if (verifications == null)
            {
                return NotFound();
            }

            return Ok(verifications);
        }

        // PUT: api/Verifications/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVerification([FromRoute] int id, [FromBody] Verification verification)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != verification.id)
            {
                return BadRequest();
            }

            _context.Entry(verification).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VerificationExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Verifications
        [HttpPost]
        public async Task<IActionResult> PostVerification([FromBody] Verification verification)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Verification.Add(verification);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVerification", new { id = verification.id }, verification);
        }

        // DELETE: api/Verifications/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVerification([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var verification = await _context.Verification.FindAsync(id);
            if (verification == null)
            {
                return NotFound();
            }

            _context.Verification.Remove(verification);
            await _context.SaveChangesAsync();

            return Ok(verification);
        }

        private bool VerificationExists(int id)
        {
            return _context.Verification.Any(e => e.id == id);
        }
    }
}