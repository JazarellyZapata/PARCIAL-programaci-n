using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // RESUELVE .Include, ToListAsync
using PARCIAL_programaci_n.Data;    // RESUELVE ApplicationDbContext
using PARCIAL_programaci_n.Models;   // RESUELVE Curso, Sesion, Asistencia
using PARCIAL_programaci_n.ViewModels; // RESUELVE AsistenciaViewModel
namespace PARCIAL_programaci_n.Controllers
{
    // Solo accesible por usuarios con el rol "Coordinador"
    [Authorize(Roles = "Coordinador")]
    public class CoordinadorController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CoordinadorController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Coordinador/Dashboard (Panel principal)
        public async Task<IActionResult> Dashboard()
        {
            // Muestra todos los cursos con conteo de matriculados
            var cursos = await _context.Cursos
                .Include(c => c.Matriculas)
                .OrderBy(c => c.Codigo)
                .ToListAsync();

            return View(cursos);
        }

        // GET: Coordinador/GestionarSesiones/5
        // Muestra sesiones existentes y permite añadir nuevas
        public async Task<IActionResult> GestionarSesiones(int id)
        {
            var curso = await _context.Cursos
                .Include(c => c.Sesiones!)
                .ThenInclude(s => s.Asistencias)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (curso == null) return NotFound();

            ViewBag.CursoNombre = curso.Nombre;
            ViewBag.CursoId = curso.Id;
            return View(curso.Sesiones!.OrderByDescending(s => s.FechaSesion).ToList());
        }

        // POST: Coordinador/CrearSesion
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearSesion(int cursoId, DateTime fechaSesion, string tema)
        {
            if (fechaSesion == default)
            {
                TempData["StatusMessage"] = "Error: La fecha de la sesión es obligatoria.";
                return RedirectToAction(nameof(GestionarSesiones), new { id = cursoId });
            }
            
            var nuevaSesion = new Sesion
            {
                CursoId = cursoId,
                FechaSesion = fechaSesion.Date, // Aseguramos que es solo la fecha
                Tema = tema
            };

            _context.Sesiones.Add(nuevaSesion);
            await _context.SaveChangesAsync();

            TempData["StatusMessage"] = $"Éxito: Sesión para el {fechaSesion.ToShortDateString()} creada.";
            return RedirectToAction(nameof(GestionarSesiones), new { id = cursoId });
        }

        // GET: Coordinador/TomarAsistencia/5/10 (CursoId / SesionId)
        public async Task<IActionResult> TomarAsistencia(int id, int sesionId)
        {
            var curso = await _context.Cursos.FirstOrDefaultAsync(c => c.Id == id);
            var sesion = await _context.Sesiones.FirstOrDefaultAsync(s => s.Id == sesionId);

            if (curso == null || sesion == null) return NotFound();

            // Obtener estudiantes matriculados en este curso
            var estudiantesMatriculados = await _context.Matriculas
                .Where(m => m.CursoId == id)
                .Include(m => m.Usuario)
                .ToListAsync();

            // Obtener asistencia previa para esta sesión
            var asistenciasPrevias = await _context.Asistencias
                .Where(a => a.SesionId == sesionId)
                .ToDictionaryAsync(a => a.UsuarioId, a => a.Asistio);

            // Crear el ViewModel para la vista
            var viewModel = estudiantesMatriculados.Select(m => new AsistenciaViewModel
            {
                UsuarioId = m.UsuarioId,
                NombreCompleto = m.Usuario!.NombreCompleto,
                Asistio = asistenciasPrevias.GetValueOrDefault(m.UsuarioId, false) // Por defecto es NO Asistió si no hay registro
            }).ToList();

            ViewBag.CursoNombre = curso.Nombre;
            ViewBag.SesionFecha = sesion.FechaSesion.ToShortDateString();
            ViewBag.SesionId = sesionId;
            ViewBag.CursoId = id;

            return View(viewModel);
        }

        // POST: Coordinador/GuardarAsistencia/10
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GuardarAsistencia(int sesionId, List<AsistenciaViewModel> asistenciaData)
        {
            var sesion = await _context.Sesiones.FirstOrDefaultAsync(s => s.Id == sesionId);
            if (sesion == null) return NotFound();

            foreach (var item in asistenciaData)
            {
                var asistencia = await _context.Asistencias
                    .FirstOrDefaultAsync(a => a.SesionId == sesionId && a.UsuarioId == item.UsuarioId);

                if (asistencia == null)
                {
                    // Crear nuevo registro si es necesario
                    if (item.Asistio)
                    {
                        _context.Asistencias.Add(new Asistencia
                        {
                            SesionId = sesionId,
                            UsuarioId = item.UsuarioId,
                            Asistio = true,
                            FechaRegistro = DateTime.Now
                        });
                    }
                }
                else
                {
                    // Actualizar registro existente
                    asistencia.Asistio = item.Asistio;
                    asistencia.FechaRegistro = DateTime.Now;
                    _context.Asistencias.Update(asistencia);
                }
            }

            await _context.SaveChangesAsync();
            
            TempData["StatusMessage"] = "Éxito: Asistencia guardada correctamente.";
            return RedirectToAction(nameof(GestionarSesiones), new { id = sesion.CursoId });
        }
    }
}