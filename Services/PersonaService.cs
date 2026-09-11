using ComAwaPot.Data;
using ComAwaPot.Models;
using Microsoft.EntityFrameworkCore;

namespace ComAwaPot.Services;

public class PersonaService
{
    private readonly DbContextOptions<AppDbContext> _dbOptions;

    public PersonaService(DbContextOptions<AppDbContext> dbOptions)
    {
        _dbOptions = dbOptions;
    }

    public async Task<List<Persona>> ObtenerTodasAsync()
    {
        using var db = new AppDbContext(_dbOptions);

        return await db.Personas
            .OrderBy(p => p.Nombre)
            .ToListAsync();
    }

    public async Task<Persona?> ObtenerPorIdAsync(int personaId)
    {
        using var db = new AppDbContext(_dbOptions);

        return await db.Personas
            .FirstOrDefaultAsync(p => p.PersonaId == personaId);
    }

    public async Task<Persona> CrearAsync(Persona persona)
    {
        using var db = new AppDbContext(_dbOptions);

        db.Personas.Add(persona);
        await db.SaveChangesAsync();

        return persona;
    }

    public async Task ActualizarAsync(Persona persona)
    {
        using var db = new AppDbContext(_dbOptions);

        db.Personas.Update(persona);
        await db.SaveChangesAsync();
    }
}
