using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace PARCIAL_programaci_n.Data
{
    // Extiende IdentityUser para agregar propiedades personalizadas (Nombre Completo)
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(100)]
        [Display(Name = "Nombre Completo")]
        public string NombreCompleto { get; set; } = string.Empty;
    }
}