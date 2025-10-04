using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PARCIAL_programaci_n.Data; 

namespace PARCIAL_programaci_n.Models 
{
    public class Matricula
    {
        // Clave Compuesta
        public int CursoId { get; set; }
        public string UsuarioId { get; set; } = string.Empty;

        [Display(Name = "Fecha de Matrícula")]
        // **CORRECCIÓN:** The dynamic assignment (DateTime.Now) must be removed.
        public DateTime FechaMatricula { get; set; } 

        // Propiedades de Navegación
        public Curso? Curso { get; set; }
        public ApplicationUser? Usuario { get; set; }
    }
}