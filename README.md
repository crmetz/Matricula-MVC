# Matrículas — Região Sul

Aplicação web para análise do número de alunos matriculados em cursos de graduação na
Região Sul do Brasil (dados do Censo da Educação Superior / MEC).

Projeto da disciplina de **Projeto e Arquitetura de Software**.

## Tecnologias / linguagens

- **C# / .NET 10**
- **ASP.NET Core MVC** (padrão arquitetural Model-View-Controller)
- **Entity Framework Core 10** (ORM)
- **PostgreSQL** (SGBD relacional) via **Npgsql**
- **CsvHelper** (leitura do CSV para o seed inicial)
- **Bootstrap 5** (interface)

## Como rodar

### 1. Subir o PostgreSQL com Docker

```bash
docker run --name matriculas-postgres -e POSTGRES_PASSWORD=postgres -e POSTGRES_DB=matriculas -p 5433:5432 -d postgres:16
```

### 2. Executar a aplicação com perfil https


Na **primeira execução**, a aplicação:

1. aplica as migrations (cria as tabelas `Cursos` e `Matriculas`);
2. roda o **seed inicial**, que lê `Data/Matriculados Região Sul.csv` e popula o banco.

O seed só é executado **se o banco ainda estiver vazio** — nas execuções seguintes ele é
ignorado. A carga inicial demora alguns instantes (são ~209 mil cursos e mais de um milhão
de registros de matrícula).

