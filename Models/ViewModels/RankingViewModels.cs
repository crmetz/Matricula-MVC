namespace Matriculas.Models.ViewModels
{
    public record RankingItem(string Nome, long Total);

    public class RankingCursosViewModel
    {
        public int Ano { get; set; } = 2023;
        public List<RankingItem> Presenciais { get; set; } = new();
        public List<RankingItem> EaD { get; set; } = new();
    }

    public class RankingIesViewModel
    {
        public int Ano { get; set; } = 2023;
        public string Categoria { get; set; } = Categorias.Publicas;
        public List<RankingItem> Presenciais { get; set; } = new();
        public List<RankingItem> EaD { get; set; } = new();
    }
}
