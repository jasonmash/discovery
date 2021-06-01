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
    public class DefectCategoriesController : ControllerBase
    {
        private readonly DataContext _context;

        public DefectCategoriesController(DataContext context)
        {
            _context = context;
        }

        // GET: api/DefectCategories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DefectCategory>>> GetDefectCategories()
        {
            return await _context.DefectCategories.ToListAsync();
        }

        // GET: api/DefectCategories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DefectCategory>> GetDefectCategory(long id)
        {
            var defectcategory = await _context.DefectCategories.FindAsync(id);

            if (defectcategory == null)
            {
                return NotFound();
            }

            return defectcategory;
        }

        // PUT: api/DefectCategories/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDefectCategory(long id, DefectCategory defectcategory)
        {
            if (id != defectcategory.Id)
            {
                return BadRequest();
            }

            _context.Entry(defectcategory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DefectCategoryExists(id)) { return NotFound(); }
            }

            return NoContent();
        }

        // POST: api/DefectCategories
        [HttpPost]
        public async Task<ActionResult<DefectCategory>> PostDefectCategory(DefectCategory defectcategory)
        {
            _context.DefectCategories.Add(defectcategory);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDefectCategory), new { id = defectcategory.Id }, defectcategory);
        }

        // DELETE: api/DefectCategories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDefectCategory(long id)
        {
            var defectcategory = await _context.DefectCategories.FindAsync(id);
            if (defectcategory == null)
            {
                return NotFound();
            }

            _context.DefectCategories.Remove(defectcategory);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DefectCategoryExists(long id)
        {
            return _context.DefectCategories.Any(e => e.Id == id);
        }
    }
}