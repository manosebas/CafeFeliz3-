// Controlador para crear ciudades.
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using System.Linq;

[Route("api/[controller]")]
[ApiController]
public class CiudadesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public CiudadesController(ApplicationDbContext context)
    {
        _context = context;
    }

    // POST: api/Ciudades
    [HttpPost]
    public async Task<ActionResult<Ciudad>> PostCiudad(Ciudad ciudad)
    {
        ciudad.Id = Guid.NewGuid();
        _context.Ciudades.Add(ciudad);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetCiudad), new { id = ciudad.Id }, ciudad);
    }

    // GET: api/Ciudades/{id}
    [HttpGet("{id}", Name = "GetCiudadById")]
    public async Task<ActionResult<Ciudad>> GetCiudad(Guid id)
    {
        var ciudad = await _context.Ciudades.FindAsync(id);

        if (ciudad == null)
        {
            return NotFound();
        }

        return ciudad;
    }

    // PUT: api/Ciudades/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> PutCiudad(Guid id, Ciudad ciudad)
    {
        if (id != ciudad.Id)
        {
            return BadRequest();
        }

        _context.Entry(ciudad).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CiudadExists(id))
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

    private bool CiudadExists(Guid id)
    {
        return _context.Ciudades.Any(e => e.Id == id);
    }
}
