using System.ComponentModel.DataAnnotations;

namespace PARCIAL_programaci_n.ViewModels
{
    // Utilizado para la vista de toma de asistencia del Coordinador (P5)
    public class AsistenciaViewModel
    {
        public string UsuarioId { get; set; } = string.Empty;
        
        [Display(Name = "Estudiante")]
        public string NombreCompleto { get; set; } = string.Empty;
        
        [Display(Name = "Asistió")]
        public bool Asistio { get; set; }
    }
}