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
            .Include(p => p.Tomas)
            .OrderBy(p => p.Nombre)
            .ToListAsync();
    }

    public async Task<Persona?> ObtenerPorIdAsync(int personaId)
    {
        using var db = new AppDbContext(_dbOptions);

        return await db.Personas
            .Include(p => p.Tomas)
            .FirstOrDefaultAsync(p => p.PersonaId == personaId);
    }

    public async Task<Persona> CrearAsync(Persona persona)
    {
        using var db = new AppDbContext(_dbOptions);

        db.Personas.Add(persona);
        await db.SaveChangesAsync();

        return persona;
    }

    public async Task<bool> CrearConPrimeraTomaAsync(
        Persona persona,
        Toma toma)
    {
        using var db = new AppDbContext(_dbOptions);

        var contratoExiste = await db.Tomas
            .AnyAsync(t => t.NumeroContrato == toma.NumeroContrato);

        if (contratoExiste)
        {
            return false;
        }

        persona.Tomas.Add(toma);

        db.Personas.Add(persona);

        db.HistorialSituaciones.Add(new HistorialSituacion
        {
            Toma = toma,
            Estado = toma.Estado,
            FechaInicio = DateTime.Today,
            FechaFin = null
        });

        await db.SaveChangesAsync();

        return true;
    }

    public async Task ActualizarAsync(Persona persona)
    {
        using var db = new AppDbContext(_dbOptions);

        db.Personas.Update(persona);
        await db.SaveChangesAsync();
    }
}
