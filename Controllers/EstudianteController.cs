using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PARCIAL_programaci_n.Data;
using PARCIAL_programaci_n.Models;
using System.Security.Claims; // Necesario para obtener el UserId

// Solo los usuarios con el rol "Estudiante" pueden acceder a este controlador
[Authorize(Roles = "Estudiante")]
public class EstudianteController : Controller
{
    private readonly ApplicationDbContext _context;

    public EstudianteController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: /Estudiante/Catalogo
    // Muestra todos los cursos disponibles
    public async Task<IActionResult> Catalogo()
    {
        // Obtener la lista de cursos disponibles
        var cursos = await _context.Cursos.ToListAsync();
        
        // Obtenemos el ID del usuario actual
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Creamos un ViewModel simple para pasar los cursos y el estado de matriculación
        var catalogo = cursos.Select(c => new CursoCatalogoViewModel
        {
            Curso = c,
            // Verificamos si el estudiante ya está matriculado en este curso
            EstaMatriculado = _context.Matriculas
                .Any(m => m.CursoId == c.Id && m.UsuarioId == userId)
        }).ToList();

        return View(catalogo);
    }

    // POST: /Estudiante/Matricularse
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Matricularse(int cursoId)
    {
        // 1. Obtener el ID del estudiante actual
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
        {
            TempData["ErrorMessage"] = "Debe iniciar sesión para matricularse.";
            return RedirectToAction(nameof(Catalogo));
        }

        // 2. Verificar si el estudiante ya está matriculado
        bool yaMatriculado = await _context.Matriculas
            .AnyAsync(m => m.CursoId == cursoId && m.UsuarioId == userId);

        if (yaMatriculado)
        {
            TempData["WarningMessage"] = "Ya estás matriculado en este curso.";
            return RedirectToAction(nameof(Catalogo));
        }

        // 3. Crear el nuevo registro de matrícula
        var matricula = new Matricula
        {
            CursoId = cursoId,
            UsuarioId = userId,
            // Se asume que FechaMatricula es una propiedad del modelo Matricula
            FechaMatricula = DateTime.Now 
        };

        _context.Matriculas.Add(matricula);
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = "Matriculación exitosa.";
        return RedirectToAction(nameof(Catalogo));
    }
}
