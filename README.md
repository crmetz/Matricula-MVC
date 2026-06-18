# Matrículas — Região Sul

Aplicação web para análise do número de alunos matriculados em cursos de graduação na
Região Sul do Brasil (dados do Censo da Educação Superior / MEC).

Projeto da disciplina de **Projeto e Arquitetura de Software**.

## Membros do grupo

- _(preencher: nome dos integrantes)_

## Tecnologias / linguagens

- **C# / .NET 10**
- **ASP.NET Core MVC** (padrão arquitetural Model-View-Controller)
- **Entity Framework Core 10** (ORM)
- **PostgreSQL** (SGBD relacional) via **Npgsql**
- **CsvHelper** (leitura do CSV para o seed inicial)
- **Bootstrap 5** (interface)

## Arquitetura e padrões

A aplicação adere ao padrão arquitetural **MVC**, com organização modular explícita:

- **Models** (`Models/`): entidades de domínio (`Curso`, `Matricula`) e ViewModels
  (`Models/ViewModels/`) que carregam os dados já preparados para cada tela.
- **Views** (`Views/`): páginas Razor (`.cshtml`) — somente apresentação.
- **Controllers** (`Controllers/`): recebem a requisição, chamam o repositório e
  devolvem a View com o ViewModel (`AnalisesController`, `HomeController`).

Para a **persistência**, é usado o padrão **Repository** (`Repositories/IMatriculaRepository`
e `MatriculaRepository`), que encapsula todas as consultas e isola os Controllers do
EF Core / PostgreSQL. O EF Core (`Data/ApplicationDbContext`) atua como ORM/mapeamento
objeto-relacional.

Fluxo: **Requisição → Controller → Repository → DbContext (EF Core) → PostgreSQL**.

### Modelo de dados (relacional, normalizado)

- `Curso` — metadados de cada oferta (Estado, Cidade, IES, Sigla, Organização,
  Categoria Administrativa, Nome do Curso, Grau, Modalidade…).
- `Matricula` — quantidade de matriculados por ano: `CursoId` (FK), `Ano`, `Quantidade`.
  Possui **chave única `(Ano, CursoId)`** e ids auto-incrementais. Cada curso tem 1—N matrículas.

## Como rodar

### 1. Subir o PostgreSQL com Docker

```bash
docker run --name matriculas-postgres -e POSTGRES_PASSWORD=postgres -e POSTGRES_DB=matriculas -p 5433:5432 -d postgres:16
```

> A porta do **host** é `5433` (mapeada para a `5432` do container) para não conflitar
> com uma instalação local do PostgreSQL que já use a porta `5432`. Se quiser usar `5432`,
> ajuste o `-p` e a `Port` na string de conexão.

A string de conexão já está configurada em `appsettings.json` apontando para esse container:

```
Host=localhost;Port=5433;Database=matriculas;Username=postgres;Password=postgres
```

### 2. Executar a aplicação

```bash
dotnet run
```

Na **primeira execução**, a aplicação:

1. aplica as migrations (cria as tabelas `Cursos` e `Matriculas`);
2. roda o **seed inicial**, que lê `Data/Matriculados Região Sul.csv` e popula o banco.

O seed só é executado **se o banco ainda estiver vazio** — nas execuções seguintes ele é
ignorado. A carga inicial demora alguns instantes (são ~209 mil cursos e mais de um milhão
de registros de matrícula).

Depois é só acessar a URL exibida no terminal (ex.: `https://localhost:xxxx`).

## Funcionalidades

- **Total por ano** — total de matriculados por ano, com filtro Todos / Presencial / EaD.
- **Ranking de cursos (2023)** — 10 cursos com mais matrículas, Presencial e EaD.
- **Ranking de IES (2023)** — 10 instituições com mais matrículas, com filtro públicas / privadas.
- **Linha do tempo de um curso** — evolução das matrículas de um curso, ano a ano, com filtro de modalidade.
