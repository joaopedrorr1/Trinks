using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Models;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProcessoController : ControllerBase
    {
        private readonly DemoDbContext _context;

        public ProcessoController(DemoDbContext context)
        {
            _context = context;
        }

        // GET: api/Processo
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Processo>>> GetProcessos()
        {
            return await _context.Processos.ToListAsync();
        }

        // GET: api/Processo/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Processo>> GetProcesso(int id)
        {
            var processo = await _context.Processos.FindAsync(id);

            if (processo == null)
            {
                return NotFound();
            }

            return processo;
        }

        // PUT: api/Processo/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProcesso(int id, Processo processo)
        {
            if (id != processo.Id)
            {
                return BadRequest();
            }

            _context.Entry(processo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProcessoExists(id))
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

        // POST: api/Processo
        [HttpPost]
        public async Task<ActionResult<Processo>> PostProcesso(Processo processo)
        {
            _context.Processos.Add(processo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProcesso", new { id = processo.Id }, processo);
        }

        // DELETE: api/Processo/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Processo>> DeleteProcesso(int id)
        {
            var processo = await _context.Processos.FindAsync(id);
            if (processo == null)
            {
                return NotFound();
            }

            _context.Processos.Remove(processo);
            await _context.SaveChangesAsync();

            return processo;
        }

        private bool ProcessoExists(int id)
        {
            return _context.Processos.Any(e => e.Id == id);
        }
    }
}
