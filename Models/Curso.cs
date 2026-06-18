namespace Matriculas.Models
{
    public class Curso
    {
        public int Id { get; set; }

        public string Estado { get; set; } = string.Empty;
        public string Cidade { get; set; } = string.Empty;
        public string IES { get; set; } = string.Empty;
        public string Sigla { get; set; } = string.Empty;
        public string Organizacao { get; set; } = string.Empty;
        public string CategoriaAdministrativa { get; set; } = string.Empty;
        public string NomeCurso { get; set; } = string.Empty;
        public string NomeDetalhadoCurso { get; set; } = string.Empty;
        public string Grau { get; set; } = string.Empty;
        public string Modalidade { get; set; } = string.Empty;

        public ICollection<Matricula> Matriculas { get; set; } = new List<Matricula>();
    }
}
