using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PARCIAL_programaci_n.Data;
using PARCIAL_programaci_n.Models;
using PARCIAL_programaci_n.Models.ViewModels; // Asegúrate de que este using exista

// El coordinador es el único que puede usar este controlador
[Authorize(Roles = "Coordinador")]
public class CoordinadorController : Controller
{
    private readonly ApplicationDbContext _context;

    public CoordinadorController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: /Coordinador/Index (Panel de control o listado de cursos)
    public async Task<IActionResult> Index()
    {
        // Muestra todos los cursos
        var cursos = await _context.Cursos
            .Include(c => c.Matriculas) // Incluir matrículas para contar estudiantes
            .ToListAsync();
            
        return View(cursos);
    }

    // GET: /Coordinador/CrearCurso (P1)
    public IActionResult CrearCurso()
    {
        return View();
    }

    // POST: /Coordinador/CrearCurso (P1)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CrearCurso(Curso curso)
    {
        if (ModelState.IsValid)
        {
            _context.Add(curso);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(curso);
    }

    // GET: /Coordinador/GestionarSesiones/5 (P4)
    public async Task<IActionResult> GestionarSesiones(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        // Obtiene el curso con sus sesiones y matrículas (para la vista)
        var curso = await _context.Cursos
            .Include(c => c.Sesiones!)
            .Include(c => c.Matriculas!)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (curso == null)
        {
            return NotFound();
        }

        return View(curso);
    }

    // POST: /Coordinador/CrearSesion (P4)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CrearSesion(int cursoId, DateTime Fecha, string Tema)
    {
        if (ModelState.IsValid)
        {
            var sesion = new Sesion
            {
                // **CORRECCIÓN DE NOMBRE**: Usamos 'Fecha' en lugar de 'FechaSesion'
                Fecha = Fecha, 
                Tema = Tema,
                CursoId = cursoId
            };
            
            _context.Add(sesion);
            await _context.SaveChangesAsync();
        }
        // Redirige de vuelta a la vista de gestión de sesiones
        return RedirectToAction(nameof(GestionarSesiones), new { id = cursoId });
    }

    // POST: /Coordinador/EliminarSesion (P4)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EliminarSesion(int sesionId)
    {
        var sesion = await _context.Sesiones.FindAsync(sesionId);
        if (sesion == null)
        {
            return NotFound();
        }

        var cursoId = sesion.CursoId;

        _context.Sesiones.Remove(sesion);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(GestionarSesiones), new { id = cursoId });
    }

    // GET: /Coordinador/TomarAsistencia/5 (P4)
    public async Task<IActionResult> TomarAsistencia(int sesionId)
    {
        var sesion = await _context.Sesiones
            .Include(s => s.Curso)
            .Include(s => s.Curso!.Matriculas!)
                .ThenInclude(m => m.Usuario)
            .Include(s => s.Asistencias)
            .FirstOrDefaultAsync(s => s.Id == sesionId);

        if (sesion == null || sesion.Curso == null)
        {
            return NotFound();
        }

        var viewModel = sesion.Curso.Matriculas!
            .Select(m => new AsistenciaViewModel
            {
                UsuarioId = m.UsuarioId,
                NombreCompleto = m.Usuario?.NombreCompleto ?? "N/A",
                // Si ya hay registro de asistencia, lo cargamos. Sino, por defecto es false.
                // **CORRECCIÓN DE NOMBRE**: Usamos 'Presente' en lugar de 'Asistio'
                Presente = sesion.Asistencias!
                    .FirstOrDefault(a => a.UsuarioId == m.UsuarioId)?.Presente ?? false 
            })
            .ToList();

        ViewData["CursoNombre"] = sesion.Curso.Nombre;
        ViewData["SesionTema"] = sesion.Tema;
        ViewData["SesionId"] = sesionId;

        return View(viewModel);
    }

    // POST: /Coordinador/GuardarAsistencia (P4)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> GuardarAsistencia(int sesionId, List<AsistenciaViewModel> asistencias)
    {
        if (!ModelState.IsValid)
        {
            return View("TomarAsistencia", asistencias);
        }

        foreach (var asistencia in asistencias)
        {
            var asistenciaExistente = await _context.Asistencias
                .FirstOrDefaultAsync(a => a.SesionId == sesionId && a.UsuarioId == asistencia.UsuarioId);

            if (asistenciaExistente != null)
            {
                // **CORRECCIÓN DE NOMBRE**: Usamos 'Presente' en lugar de 'Asistio'
                asistenciaExistente.Presente = asistencia.Presente; 
                
                // **ELIMINACIÓN DE ERROR**: Quitamos la referencia incorrecta a FechaMatricula
            }
            else if (asistencia.Presente) // Solo creamos nuevo registro si está Presente
            {
                var nuevaAsistencia = new Asistencia
                {
                    SesionId = sesionId,
                    UsuarioId = asistencia.UsuarioId,
                    // **CORRECCIÓN DE NOMBRE**: Usamos 'Presente' en lugar de 'Asistio'
                    Presente = asistencia.Presente, 
                    // **ELIMINACIÓN DE ERROR**: Quitamos la referencia incorrecta a FechaMatricula
                };

                _context.Asistencias.Add(nuevaAsistencia);
            }
        }

        await _context.SaveChangesAsync();
        TempData["StatusMessage"] = "Asistencia guardada correctamente.";

        var cursoId = (await _context.Sesiones.FindAsync(sesionId))?.CursoId;
        return RedirectToAction(nameof(GestionarSesiones), new { id = cursoId });
    }
}