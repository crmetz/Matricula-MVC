using Matriculas.Models;
using Matriculas.Models.ViewModels;
using Matriculas.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Matriculas.Controllers
{
    public class AnalisesController(IMatriculaRepository repository) : Controller
    {
        private const int AnoBase = 2023;
        private readonly IMatriculaRepository _repository = repository;

        public async Task<IActionResult> TotalPorAno(string modalidade = Modalidades.Todos)
        {
            var vm = new TotalPorAnoViewModel
            {
                Modalidade = modalidade,
                Totais = await _repository.TotalPorAnoAsync(modalidade)
            };
            return View(vm);
        }

        public async Task<IActionResult> RankingCursos()
        {
            var vm = new RankingCursosViewModel
            {
                Ano = AnoBase,
                Presenciais = await _repository.TopCursosAsync(Modalidades.Presencial, AnoBase),
                EaD = await _repository.TopCursosAsync(Modalidades.EaD, AnoBase)
            };
            return View(vm);
        }

        public async Task<IActionResult> RankingIes(string categoria = Categorias.Publicas)
        {
            var vm = new RankingIesViewModel
            {
                Ano = AnoBase,
                Categoria = categoria,
                Presenciais = await _repository.TopIesAsync(Modalidades.Presencial, categoria, AnoBase),
                EaD = await _repository.TopIesAsync(Modalidades.EaD, categoria, AnoBase)
            };
            return View(vm);
        }

        public async Task<IActionResult> LinhaDoTempo(string? nomeCurso, string modalidade = Modalidades.Todos)
        {
            var vm = new LinhaDoTempoViewModel
            {
                CursosDisponiveis = await _repository.ListarNomesCursosAsync(),
                NomeCurso = nomeCurso,
                Modalidade = modalidade
            };

            if (!string.IsNullOrWhiteSpace(nomeCurso))
                vm.Serie = await _repository.LinhaDoTempoCursoAsync(nomeCurso, modalidade);

            return View(vm);
        }
    }
}
