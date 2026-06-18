namespace Matriculas.Models.ViewModels
{
    public class LinhaDoTempoViewModel
    {
        public List<string> CursosDisponiveis { get; set; } = new();
        public string? NomeCurso { get; set; }
        public string Modalidade { get; set; } = Modalidades.Todos;
        public List<AnoTotal> Serie { get; set; } = new();
    }
}
