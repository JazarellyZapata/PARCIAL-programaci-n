using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PARCIAL_programaci_n.Data;
using PARCIAL_programaci_n.Models;

// Este controlador es para el Estudiante y debe requerir autenticación.
[Authorize(Roles = "Estudiante")]
public class EstudianteController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public EstudianteController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // GET: /Estudiante/Catalogo
    // Muestra todos los cursos activos y disponibles para matricularse.
    public async Task<IActionResult> Catalogo()
    {
        // Obtener el ID del usuario logueado
        var userId = _userManager.GetUserId(User);

        // Obtener todos los cursos activos
        var cursosActivos = await _context.Cursos
            .Where(c => c.Activo)
            .Include(c => c.Matriculas!) // Incluye las matrículas
            .ToListAsync();

        // Obtener los IDs de los cursos en los que el usuario ya está matriculado
        var cursosMatriculadosIds = await _context.Matriculas
            .Where(m => m.UsuarioId == userId)
            .Select(m => m.CursoId)
            .ToListAsync();

        // Puedes usar el ViewModel del curso o ViewBag para pasar el estado de matrícula
        ViewBag.CursosMatriculados = cursosMatriculadosIds;
        
        // Retorna la lista de cursos
        return View(cursosActivos);
    }

    // GET: /Estudiante/MisMatriculas
    // Muestra los cursos en los que el estudiante está inscrito.
    public async Task<IActionResult> MisMatriculas()
    {
        var userId = _userManager.GetUserId(User);

        // Obtener las matrículas del usuario e incluir la información del Curso y Sesiones
        var matriculas = await _context.Matriculas
            .Where(m => m.UsuarioId == userId)
            .Include(m => m.Curso!)
                .ThenInclude(c => c.Sesiones) // P4: Incluir las sesiones para cada curso
            .ToListAsync();

        return View(matriculas);
    }

    // POST: /Estudiante/Matricularse/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Matricularse(int cursoId)
    {
        var userId = _userManager.GetUserId(User);
        
        // 1. Verificar si ya está matriculado
        if (_context.Matriculas.Any(m => m.CursoId == cursoId && m.UsuarioId == userId))
        {
            TempData["StatusMessage"] = "Error: Ya estás matriculado en este curso.";
            return RedirectToAction(nameof(Catalogo));
        }

        // 2. Verificar el cupo (Ejemplo simple)
        var curso = await _context.Cursos.Include(c => c.Matriculas).FirstOrDefaultAsync(c => c.Id == cursoId);
        if (curso == null) return NotFound();

        if (curso.Matriculas!.Count >= curso.CupoMaximo)
        {
            TempData["StatusMessage"] = "Error: El curso ha alcanzado su cupo máximo.";
            return RedirectToAction(nameof(Catalogo));
        }

        // 3. Crear la nueva matrícula
        var matricula = new Matricula
        {
            CursoId = cursoId,
            UsuarioId = userId!,
            FechaMatricula = DateTime.Now
        };

        _context.Matriculas.Add(matricula);
        await _context.SaveChangesAsync();

        TempData["StatusMessage"] = "Éxito: Te has matriculado correctamente.";
        return RedirectToAction(nameof(MisMatriculas));
    }
}