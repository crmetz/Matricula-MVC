namespace Matriculas.Models
{
    public class Matricula
    {
        public int Id { get; set; }

        public int CursoId { get; set; }
        public Curso? Curso { get; set; }

        public int Ano { get; set; }
        public int Quantidade { get; set; }
    }
}
