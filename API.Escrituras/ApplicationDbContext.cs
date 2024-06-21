using Microsoft.EntityFrameworkCore;
using System;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Ciudad> Ciudades { get; set; }
    public DbSet<Cliente> Clientes { get; set; }
}

public class Ciudad
{
    public Guid Id { get; set; }
    public string Nombre { get; set; }
}

public class Cliente
{
    public Guid Id { get; set; }
    public string Nombre { get; set; }
    public string Email { get; set; }
    public Guid IdCiudad { get; set; }
}
