using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PARCIAL_PROGRAMACI_N.Data;
using PARCIAL_PROGRAMACI_N.Models;

// Solo el rol Coordinador puede acceder a este controlador (P2)
[Authorize(Roles = "Coordinador")]
public class CursosController : Controller
{
    private readonly ApplicationDbContext _context;

    public CursosController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: /Cursos (P2)
    public async Task<IActionResult> Index()
    {
        // Muestra todos los cursos
        return View(await _context.Cursos.ToListAsync());
    }

    // GET: /Cursos/Create (P2)
    public IActionResult Create()
    {
        return View();
    }

    // POST: /Cursos/Create (P2)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Curso curso)
    {
        // 1. Validación de Horarios (P2)
        if (curso.HorarioInicio >= curso.HorarioFin)
        {
            ModelState.AddModelError("HorarioFin", "La hora de finalización debe ser posterior a la hora de inicio.");
        }

        // 2. Validación de Unicidad del Código (P2)
        if (await _context.Cursos.AnyAsync(c => c.Codigo == curso.Codigo))
        {
            ModelState.AddModelError("Codigo", "Ya existe un curso con este código.");
        }
        
        // El resto de validaciones (Required, Range) se manejan con DataAnnotations.

        if (ModelState.IsValid)
        {
            _context.Add(curso);
            await _context.SaveChangesAsync();
            TempData["MensajeExito"] = $"Curso {curso.Codigo} creado con éxito.";
            return RedirectToAction(nameof(Index));
        }
        return View(curso);
    }

    // GET: /Cursos/Edit/5 (P2)
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var curso = await _context.Cursos.FindAsync(id);
        if (curso == null) return NotFound();

        return View(curso);
    }

    // POST: /Cursos/Edit/5 (P2)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Curso curso)
    {
        if (id != curso.Id) return NotFound();

        // 1. Validación de Horarios (P2)
        if (curso.HorarioInicio >= curso.HorarioFin)
        {
            ModelState.AddModelError("HorarioFin", "La hora de finalización debe ser posterior a la hora de inicio.");
        }

        // 2. Validación de Unicidad del Código (Excluyendo el curso actual) (P2)
        if (await _context.Cursos.AnyAsync(c => c.Codigo == curso.Codigo && c.Id != id))
        {
            ModelState.AddModelError("Codigo", "Ya existe otro curso con este código.");
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(curso);
                await _context.SaveChangesAsync();
                TempData["MensajeExito"] = $"Curso {curso.Codigo} actualizado con éxito.";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Cursos.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                throw;
            }
            return RedirectToAction(nameof(Index));
        }
        return View(curso);
    }

    // POST: /Cursos/ToggleStatus/5 (P2)
    // Cambia el estado Activo/Inactivo
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleStatus(int id)
    {
        var curso = await _context.Cursos.FindAsync(id);

        if (curso == null)
        {
            TempData["MensajeError"] = "Curso no encontrado.";
            return RedirectToAction(nameof(Index));
        }

        curso.Activo = !curso.Activo;
        _context.Update(curso);
        await _context.SaveChangesAsync();

        string estado = curso.Activo ? "activado" : "desactivado";
        TempData["MensajeExito"] = $"Curso {curso.Codigo} ha sido {estado}.";
        return RedirectToAction(nameof(Index));
    }
}