namespace Matriculas.Models.ViewModels
{
    public class TotalPorAnoViewModel
    {
        public string Modalidade { get; set; } = Modalidades.Todos;
        public List<AnoTotal> Totais { get; set; } = new();
    }

    public record AnoTotal(int Ano, long Total);
}
