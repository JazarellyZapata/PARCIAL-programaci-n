using Microsoft.AspNetCore.Identity;

// *** IMPORTANTE: Asegúrate que este namespace coincida con el nombre de tu proyecto ***
// El nombre del proyecto (con guiones bajos en lugar de guiones) + .Data
namespace PARCIAL_PROGRAMACI_N.Data
{
    // ApplicationUser extiende la clase base IdentityUser,
    // que es la representación de un usuario en el sistema.
    public class ApplicationUser : IdentityUser
    {
        // Por ahora no necesitamos propiedades adicionales,
        // pero se podrían agregar aquí si el examen lo requiriera.
    }
}
