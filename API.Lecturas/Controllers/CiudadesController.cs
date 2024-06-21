// Controlador para obtener la lista de ciudades y la cantidad de clientes por ciudad.
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class CiudadesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public CiudadesController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/Ciudades
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Ciudad>>> GetCiudades()
    {
        return await _context.Ciudades.ToListAsync();
    }

    // GET: api/Ciudades/ClientesCount
    [HttpGet("ClientesCount")]
    public async Task<ActionResult<IEnumerable<object>>> GetCiudadesWithClientesCount()
    {
        var ciudadesConClientes = await _context.Ciudades
            .Select(ciudad => new
            {
                Ciudad = ciudad.Nombre,
                ClientesCount = _context.Clientes.Count(cliente => cliente.IdCiudad == ciudad.Id)
            })
            .ToListAsync();

        return Ok(ciudadesConClientes);
    }

}
