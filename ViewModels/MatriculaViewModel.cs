using PARCIAL_programaci_n.Models;

namespace PARCIAL_programaci_n.ViewModels
{
    public class MatriculaViewModel
    {
        public int CursoId { get; set; }
        public string? Codigo { get; set; }
        public string? Nombre { get; set; }
        public int Creditos { get; set; }
        public DateTime FechaMatricula { get; set; }
        
        // Propiedades de la Pregunta 4
        public int SesionesTotales { get; set; }
        public int Asistencias { get; set; }
        public double PorcentajeAsistencia { get; set; }
        public bool CreditosObtenidos => PorcentajeAsistencia >= 75.0; // Se asume 75% para obtener créditos
    }
}