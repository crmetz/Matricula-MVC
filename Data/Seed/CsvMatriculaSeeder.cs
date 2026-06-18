using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using Matriculas.Models;
using Microsoft.EntityFrameworkCore;

namespace Matriculas.Data.Seed
{
    // Popula Cursos e Matriculas a partir do CSV; só roda se o banco estiver vazio.
    public class CsvMatriculaSeeder(
        ApplicationDbContext context,
        IWebHostEnvironment env,
        ILogger<CsvMatriculaSeeder> logger)
    {
        private static readonly int[] Anos =
            { 2009, 2010, 2011, 2012, 2013, 2014, 2015, 2016, 2017, 2018, 2019, 2020, 2021, 2022, 2023 };

        private const int TamanhoLote = 2000;

        private readonly ApplicationDbContext _context = context;
        private readonly IWebHostEnvironment _env = env;
        private readonly ILogger<CsvMatriculaSeeder> _logger = logger;

        public async Task SeedAsync()
        {
            if (await _context.Cursos.AnyAsync())
            {
                _logger.LogInformation("Seed ignorado: já existem dados no banco.");
                return;
            }

            var caminho = Path.Combine(_env.ContentRootPath, "Data", "Matriculados Região Sul.csv");
            if (!File.Exists(caminho))
            {
                _logger.LogWarning("Arquivo CSV não encontrado em {Caminho}. Seed abortado.", caminho);
                return;
            }

            _logger.LogInformation("Iniciando seed a partir de {Caminho}...", caminho);
            var inicio = DateTime.UtcNow;

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";",
                HasHeaderRecord = true,
                MissingFieldFound = null,
                BadDataFound = null
            };

            _context.ChangeTracker.AutoDetectChangesEnabled = false;

            int totalCursos = 0;
            long totalMatriculas = 0;
            var lote = new List<Curso>(TamanhoLote);

            using (var reader = new StreamReader(caminho))
            using (var csv = new CsvReader(reader, config))
            {
                await csv.ReadAsync();
                csv.ReadHeader();

                while (await csv.ReadAsync())
                {
                    var curso = new Curso
                    {
                        Estado = csv.GetField("Estado") ?? string.Empty,
                        Cidade = csv.GetField("Cidade") ?? string.Empty,
                        IES = csv.GetField("IES") ?? string.Empty,
                        Sigla = csv.GetField("Sigla") ?? string.Empty,
                        Organizacao = csv.GetField("Organização") ?? string.Empty,
                        CategoriaAdministrativa = csv.GetField("Categoria Administrativa") ?? string.Empty,
                        NomeCurso = csv.GetField("Nome do Curso") ?? string.Empty,
                        NomeDetalhadoCurso = csv.GetField("Nome Detalhado do Curso") ?? string.Empty,
                        Grau = csv.GetField("Grau") ?? string.Empty,
                        Modalidade = csv.GetField("Modalidade") ?? string.Empty
                    };

                    foreach (var ano in Anos)
                    {
                        var bruto = csv.GetField(ano.ToString());
                        if (int.TryParse(bruto, NumberStyles.Integer, CultureInfo.InvariantCulture, out var qtd))
                        {
                            curso.Matriculas.Add(new Matricula { Ano = ano, Quantidade = qtd });
                            totalMatriculas++;
                        }
                    }

                    lote.Add(curso);
                    totalCursos++;

                    if (lote.Count >= TamanhoLote)
                    {
                        await PersistirLoteAsync(lote);
                        lote.Clear();
                    }
                }
            }

            if (lote.Count > 0)
                await PersistirLoteAsync(lote);

            var duracao = DateTime.UtcNow - inicio;
            _logger.LogInformation(
                "Seed concluído: {Cursos} cursos e {Matriculas} matrículas em {Segundos:N1}s.",
                totalCursos, totalMatriculas, duracao.TotalSeconds);
        }

        private async Task PersistirLoteAsync(List<Curso> lote)
        {
            _context.Cursos.AddRange(lote);
            await _context.SaveChangesAsync();
            _context.ChangeTracker.Clear();
        }
    }
}
