using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // <-- Soluciona 'EntityFrameworkCore' no existe
using PARCIAL_programaci_n.Models;// <-- Asegúrate de que este namespace es correcto (Contiene ApplicationDbContext y ApplicationUser)
using PARCIAL_programaci_n.Data; // <--- DEBE coincidir con el namespace de la carpeta Data
using System.Security.Claims;


namespace PARCIAL_programaci_n.Controllers
{
    // Solo accesible a usuarios autenticados (Estudiantes o Coordinadores)
    [Authorize] 
    public class EstudianteController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public EstudianteController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Estudiante/Catalogo
        // Muestra los cursos activos en los que el estudiante no está matriculado.
        public async Task<IActionResult> Catalogo()
        {
            // Redirige al Coordinador a su panel
            if (User.IsInRole("Coordinador"))
            {
                return RedirectToAction("Index", "Cursos");
            }
            
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // 1. Obtener los IDs de los cursos en los que el usuario YA está matriculado
            var matriculatedCourseIds = await _context.Matriculas
                .Where(m => m.UsuarioId == userId)
                .Select(m => m.CursoId)
                .ToListAsync();

            // 2. Obtener los cursos activos que NO están en la lista de matriculados
            var availableCourses = await _context.Cursos
                .Where(c => c.Activo && !matriculatedCourseIds.Contains(c.Id))
                .OrderBy(c => c.Codigo)
                .ToListAsync();
            
            return View(availableCourses);
        }
        
        // POST: Estudiante/Matricularse/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Matricularse(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var curso = await _context.Cursos
                .Include(c => c.Matriculas) // Cargar las matrículas para contar
                .FirstOrDefaultAsync(c => c.Id == id);

            if (curso == null || !curso.Activo)
            {
                TempData["StatusMessage"] = "Error: Curso no encontrado o inactivo.";
                return RedirectToAction(nameof(Catalogo));
            }

            // Validación 1: ¿Ya está matriculado?
            if (_context.Matriculas.Any(m => m.CursoId == id && m.UsuarioId == userId))
            {
                TempData["StatusMessage"] = $"Error: Ya estás matriculado en el curso {curso.Codigo} - {curso.Nombre}.";
                return RedirectToAction(nameof(Catalogo));
            }

            // Validación 2: ¿Hay cupo disponible?
            if (curso.Matriculas.Count >= curso.CupoMaximo)
            {
                TempData["StatusMessage"] = $"Error: El curso {curso.Codigo} - {curso.Nombre} ha alcanzado su cupo máximo de {curso.CupoMaximo} estudiantes.";
                return RedirectToAction(nameof(Catalogo));
            }

            // 3. Crear y guardar la nueva matrícula
            var matricula = new Matricula
            {
                CursoId = curso.Id,
                UsuarioId = userId,
                FechaMatricula = DateTime.Now 
            };

            _context.Matriculas.Add(matricula);
            await _context.SaveChangesAsync();

            TempData["StatusMessage"] = $"¡Éxito! Te has matriculado en el curso {curso.Codigo} - {curso.Nombre}.";
            return RedirectToAction(nameof(Matriculas));
        }

        // GET: Estudiante/Matriculas
        // Muestra el listado de cursos en los que el estudiante SÍ está matriculado.
        public async Task<IActionResult> Matriculas()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var matriculas = await _context.Matriculas
                .Where(m => m.UsuarioId == userId)
                .Include(m => m.Curso)
                .OrderByDescending(m => m.FechaMatricula)
                .ToListAsync();

            // Redirige al Coordinador a su panel
            if (User.IsInRole("Coordinador"))
            {
                return RedirectToAction("Index", "Cursos");
            }

            return View(matriculas);
        }

        // POST: Estudiante/CancelarMatricula/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelarMatricula(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var matricula = await _context.Matriculas
                .Include(m => m.Curso)
                .FirstOrDefaultAsync(m => m.CursoId == id && m.UsuarioId == userId);

            if (matricula == null)
            {
                TempData["StatusMessage"] = "Error: No se encontró la matrícula a cancelar.";
                return RedirectToAction(nameof(Matriculas));
            }

            string cursoCodigo = matricula.Curso.Codigo;
            string cursoNombre = matricula.Curso.Nombre;

            _context.Matriculas.Remove(matricula);
            await _context.SaveChangesAsync();

            TempData["StatusMessage"] = $"¡Matrícula cancelada! Se ha dado de baja del curso {cursoCodigo} - {cursoNombre}.";
            return RedirectToAction(nameof(Matriculas));
        }
    }
}