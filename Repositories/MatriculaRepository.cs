using Matriculas.Data;
using Matriculas.Models;
using Matriculas.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Matriculas.Repositories
{
    public class MatriculaRepository(ApplicationDbContext context) : IMatriculaRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<List<AnoTotal>> TotalPorAnoAsync(string modalidade)
        {
            var query = AplicarModalidade(_context.Matriculas, modalidade);

            // Soma como int (traduzível para SQL) e converte para long no cliente.
            var dados = await query
                .GroupBy(m => m.Ano)
                .Select(g => new { Ano = g.Key, Total = g.Sum(m => m.Quantidade) })
                .OrderBy(x => x.Ano)
                .ToListAsync();

            return dados.Select(x => new AnoTotal(x.Ano, x.Total)).ToList();
        }

        public async Task<List<RankingItem>> TopCursosAsync(string modalidade, int ano, int top = 10)
        {
            var query = AplicarModalidade(_context.Matriculas, modalidade)
                .Where(m => m.Ano == ano);

            var dados = await query
                .GroupBy(m => m.Curso!.NomeCurso)
                .Select(g => new { Nome = g.Key, Total = g.Sum(m => m.Quantidade) })
                .OrderByDescending(x => x.Total)
                .Take(top)
                .ToListAsync();

            return dados.Select(x => new RankingItem(x.Nome, x.Total)).ToList();
        }

        public async Task<List<RankingItem>> TopIesAsync(string modalidade, string categoria, int ano, int top = 10)
        {
            var query = AplicarModalidade(_context.Matriculas, modalidade)
                .Where(m => m.Ano == ano);

            query = AplicarCategoria(query, categoria);

            var dados = await query
                .GroupBy(m => m.Curso!.IES)
                .Select(g => new { Nome = g.Key, Total = g.Sum(m => m.Quantidade) })
                .OrderByDescending(x => x.Total)
                .Take(top)
                .ToListAsync();

            return dados.Select(x => new RankingItem(x.Nome, x.Total)).ToList();
        }

        public async Task<List<string>> ListarNomesCursosAsync()
        {
            return await _context.Cursos
                .Select(c => c.NomeCurso)
                .Distinct()
                .OrderBy(nome => nome)
                .ToListAsync();
        }

        public async Task<List<AnoTotal>> LinhaDoTempoCursoAsync(string nomeCurso, string modalidade)
        {
            var query = AplicarModalidade(_context.Matriculas, modalidade)
                .Where(m => m.Curso!.NomeCurso == nomeCurso);

            var dados = await query
                .GroupBy(m => m.Ano)
                .Select(g => new { Ano = g.Key, Total = g.Sum(m => m.Quantidade) })
                .OrderBy(x => x.Ano)
                .ToListAsync();

            return dados.Select(x => new AnoTotal(x.Ano, x.Total)).ToList();
        }

        private static IQueryable<Matricula> AplicarModalidade(IQueryable<Matricula> query, string modalidade)
        {
            if (modalidade == Modalidades.Presencial || modalidade == Modalidades.EaD)
                return query.Where(m => m.Curso!.Modalidade == modalidade);

            return query;
        }

        private static IQueryable<Matricula> AplicarCategoria(IQueryable<Matricula> query, string categoria)
        {
            if (categoria == Categorias.Publicas)
                return query.Where(m => m.Curso!.CategoriaAdministrativa.StartsWith("Pública"));

            if (categoria == Categorias.Privadas)
                return query.Where(m => m.Curso!.CategoriaAdministrativa.StartsWith("Privada"));

            return query;
        }
    }
}
