using Microsoft.AspNetCore.Identity;
using PARCIAL_programaci_n.Models; // <--- ¡AÑADE ESTA LÍNEA!
using System.ComponentModel.DataAnnotations;

namespace PARCIAL_programaci_n.Data
{
    // Heredamos de IdentityUser para extender el modelo base de usuario
    public class ApplicationUser : IdentityUser
    {
        [Required(ErrorMessage = "El nombre completo es obligatorio.")]
        [StringLength(100)]
        [Display(Name = "Nombre Completo")]
        public string NombreCompleto { get; set; } = string.Empty;
        
        // Propiedad de navegación para las matrículas (uno a muchos)
        public ICollection<Matricula>? Matriculas { get; set; }
    }
}