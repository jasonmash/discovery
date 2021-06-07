using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Discovery.Data;

namespace Discovery.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DefectsController : ControllerBase
    {
        private readonly DataContext _context;

        public DefectsController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Defects
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Defect>>> GetDefects()
        {
            return await _context.Defects.ToListAsync();
        }

        // GET: api/Defects/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Defect>> GetDefect(int id)
        {
            var defect = await _context.Defects.FindAsync(id);

            if (defect == null)
            {
                return NotFound();
            }

            return defect;
        }

        // PUT: api/Defects/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDefect(int id, Defect defect)
        {
            if (id != defect.Id)
            {
                return BadRequest();
            }

            _context.Entry(defect).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DefectExists(id)) { return NotFound(); }
            }

            return NoContent();
        }

        // POST: api/Defects
        [HttpPost]
        public async Task<ActionResult<Defect>> PostDefect(Defect defect)
        {
            _context.Defects.Add(defect);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDefect), new { id = defect.Id }, defect);
        }

        // DELETE: api/Defects/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDefect(int id)
        {
            var defect = await _context.Defects.FindAsync(id);
            if (defect == null)
            {
                return NotFound();
            }

            _context.Defects.Remove(defect);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DefectExists(int id)
        {
            return _context.Defects.Any(e => e.Id == id);
        }
    }
}