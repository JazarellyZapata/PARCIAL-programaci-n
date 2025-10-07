using PARCIAL_programaci_n.Models;

namespace PARCIAL_programaci_n.Models.ViewModels
{
    // Modelo usado por el EstudianteController para el catálogo
    public class CursoCatalogoViewModel
    {
        public Curso Curso { get; set; } = new Curso();
        // Propiedad que indica si el estudiante actual ya tomó el curso
        public bool EstaMatriculado { get; set; }
    }
}
