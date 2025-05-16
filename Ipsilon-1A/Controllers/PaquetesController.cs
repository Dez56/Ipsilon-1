using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ipsilon_1A.Data;
using Ipsilon_1A.Models;

namespace Ipsilon_1A.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PaquetesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PaquetesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Paquetes
        [HttpGet("All")]
        public async Task<ActionResult<IEnumerable<Paquete>>> GetPaquetesAll()
        {
            return await _context.Paquetes.ToListAsync();
        }

        // GET: /Paquetes?skip=0&take=10
        [HttpGet]
        public IActionResult GetPaquetes([FromQuery] int skip, [FromQuery] int take)
        {
            var paquetes = _context.Paquetes
                .OrderBy(p => p.Id)
                .Skip(skip)
                .Take(take)
                .ToList();

            return Ok(paquetes);
        }

        // GET: /Paquetes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Paquete>> GetPaquete(int id)
        {
            var paquete = await _context.Paquetes.FindAsync(id);

            if (paquete == null)
            {
                return NotFound();
            }

            return paquete;
        }

        // GET: /Paquetes/BuscarPorCodigo/{codigo}
        [HttpGet("BuscarPorCodigo/{codigo}")]
        public async Task<ActionResult<Paquete>> BuscarPorCodigo(string codigo)
        {
            var paquete = await _context.Paquetes
                .FirstOrDefaultAsync(p => p.Codigo == codigo);

            if (paquete == null)
                return NotFound();

            return Ok(paquete);
        }

        // PUT: /Paquetes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPaquete(int id, Paquete paquete)
        {
            if (id != paquete.Id)
            {
                return BadRequest();
            }

            _context.Entry(paquete).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaqueteExists(id))
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

        // POST: /Paquetes
        [HttpPost]
        public async Task<ActionResult<Paquete>> PostPaquete(Paquete paquete)
        {
            _context.Paquetes.Add(paquete);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPaquete), new { id = paquete.Id }, paquete);
        }

        // DELETE: /Paquetes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePaquete(int id)
        {
            var paquete = await _context.Paquetes.FindAsync(id);
            if (paquete == null)
            {
                return NotFound();
            }

            _context.Paquetes.Remove(paquete);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PaqueteExists(int id)
        {
            return _context.Paquetes.Any(e => e.Id == id);
        }
    }
}
