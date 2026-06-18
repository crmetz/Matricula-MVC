using Matriculas.Models.ViewModels;

namespace Matriculas.Repositories
{
    // Padrão Repository: isola os Controllers do acesso a dados (EF Core / PostgreSQL).
    public interface IMatriculaRepository
    {
        Task<List<AnoTotal>> TotalPorAnoAsync(string modalidade);

        Task<List<RankingItem>> TopCursosAsync(string modalidade, int ano, int top = 10);

        Task<List<RankingItem>> TopIesAsync(string modalidade, string categoria, int ano, int top = 10);

        Task<List<string>> ListarNomesCursosAsync();

        Task<List<AnoTotal>> LinhaDoTempoCursoAsync(string nomeCurso, string modalidade);
    }
}
