using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PARCIAL_programaci_n.Data;
using PARCIAL_programaci_n.Models;

// Solo el Coordinador puede acceder a esta gestión
[Authorize(Roles = "Coordinador")]
public class CursosController : Controller
{
    private readonly ApplicationDbContext _context;

    public CursosController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Cursos (Muestra la lista de cursos)
    public async Task<IActionResult> Index()
    {
        return View(await _context.Cursos.ToListAsync());
    }

    // GET: Cursos/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var curso = await _context.Cursos
            .Include(c => c.Matriculas!)
            .ThenInclude(m => m.Usuario)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (curso == null) return NotFound();

        return View(curso);
    }

    // GET: Cursos/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Cursos/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Codigo,Nombre,Creditos,CupoMaximo,Activo")] Curso curso)
    {
        // Nota: Si usas .NET 8 o superior, el ModelState es true por defecto en la plantilla MVC
        if (ModelState.IsValid)
        {
            _context.Add(curso);
            await _context.SaveChangesAsync();
            TempData["StatusMessage"] = $"Éxito: El curso {curso.Codigo} fue creado.";
            return RedirectToAction(nameof(Index));
        }
        return View(curso);
    }

    // GET: Cursos/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var curso = await _context.Cursos.FindAsync(id);
        if (curso == null) return NotFound();
        return View(curso);
    }

    // POST: Cursos/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Codigo,Nombre,Creditos,CupoMaximo,Activo")] Curso curso)
    {
        if (id != curso.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(curso);
                await _context.SaveChangesAsync();
                TempData["StatusMessage"] = $"Éxito: El curso {curso.Codigo} fue actualizado.";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CursoExists(curso.Id)) return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));
        }
        return View(curso);
    }

    // GET: Cursos/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var curso = await _context.Cursos
            .FirstOrDefaultAsync(m => m.Id == id);
        if (curso == null) return NotFound();

        return View(curso);
    }

    // POST: Cursos/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var curso = await _context.Cursos.FindAsync(id);
        if (curso != null)
        {
            _context.Cursos.Remove(curso);
        }
        
        await _context.SaveChangesAsync();
        TempData["StatusMessage"] = $"Éxito: El curso fue eliminado.";
        return RedirectToAction(nameof(Index));
    }

    private bool CursoExists(int id)
    {
        return _context.Cursos.Any(e => e.Id == id);
    }
}