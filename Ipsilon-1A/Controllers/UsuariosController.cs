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
    [ApiController]
    [Route("[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UsuariosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Usuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            return await _context.Usuarios.ToListAsync();
        }

        // GET: /Usuarios/paged?skip=0&take=10
        [HttpGet("paged")]
        public IActionResult GetUsuariosPaged(int skip, int take)
        {
            var usuarios = _context.Usuarios
                .OrderBy(u => u.Id)
                .Skip(skip)
                .Take(take)
                .ToList();

            return Ok(usuarios);
        }

        // GET: /Usuarios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }

            return usuario;
        }

        // GET: /Usuarios/ByName?nombre=...&contrasena=...
        [HttpGet("ByName")]
        public async Task<ActionResult<Usuario>> GetUsuarioByName(string nombre, string contrasena)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Nombre == nombre && u.Contrasena == contrasena);

            if (usuario == null)
            {
                return NotFound();
            }

            return usuario;
        }

        // POST: /Usuarios
        [HttpPost]
        public async Task<ActionResult<Usuario>> PostUsuario(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUsuario", new { id = usuario.Id }, usuario);
        }

        // PUT: /Usuarios/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(int id, Usuario usuario)
        {
            if (id != usuario.Id)
            {
                return BadRequest();
            }

            _context.Entry(usuario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(id))
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

        // DELETE: /Usuarios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.Id == id);
        }
    }
}
