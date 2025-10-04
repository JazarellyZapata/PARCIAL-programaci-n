using Microsoft.AspNetCore.Identity;
using PARCIAL_programaci_n.Models;
using System.ComponentModel.DataAnnotations;

namespace PARCIAL_programaci_n.Data
{
    // Extiende la clase IdentityUser para añadir propiedades personalizadas
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(100)]
        [Display(Name = "Nombre Completo")]
        public string NombreCompleto { get; set; } = "Estudiante Genérico";
        
        // Propiedad de navegación para las matrículas del usuario (P3)
        public ICollection<Matricula>? Matriculas { get; set; }
    }
}