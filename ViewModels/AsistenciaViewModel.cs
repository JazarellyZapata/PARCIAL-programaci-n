using System.ComponentModel.DataAnnotations;

namespace PARCIAL_programaci_n.Models.ViewModels
{
    // Este ViewModel se usa en el CoordinadorController para mostrar
    // el estado de asistencia de cada estudiante matriculado.
    public class AsistenciaViewModel
    {
        [Required]
        public string UsuarioId { get; set; } = string.Empty;
        
        [Display(Name = "Estudiante")]
        public string NombreCompleto { get; set; } = string.Empty;
        
        [Display(Name = "Presente")]
        // **IMPORTANTE**: Esta propiedad resuelve los errores CS1061/CS0117 anteriores.
        public bool Presente { get; set; }
    }
}