# Instruções de Configuração - Gestão de Voluntariado

## Configuração Inicial do Banco de Dados

### Opção 1: Execução Automática (Recomendado)
O banco de dados é criado automaticamente quando você executa a aplicação pela primeira vez. Não é necessário executar migrations manualmente.

**Passos:**
1. Abra o projeto no Visual Studio
2. Pressione F5 para executar
3. O arquivo `gestaovoluntariado.db` será criado automaticamente

### Opção 2: Criar Migrations Manualmente

Se você quiser criar migrations explicitamente (por exemplo, após modificar o modelo):

#### Passo 1: Abrir Package Manager Console
- No Visual Studio: `Tools > NuGet Package Manager > Package Manager Console`

#### Passo 2: Criar a Migration Inicial
```powershell
Add-Migration InitialCreate
```

#### Passo 3: Aplicar a Migration ao Banco de Dados
```powershell
Update-Database
```

### Opção 3: Usando .NET CLI

Se preferir usar a linha de comando:

#### Passo 1: Criar a Migration
```bash
dotnet ef migrations add InitialCreate
```

#### Passo 2: Aplicar a Migration
```bash
dotnet ef database update
```

## Estrutura do Banco de Dados

Após a execução, o banco de dados conterá as seguintes tabelas:

### Tabela: Organizations
```sql
CREATE TABLE Organizations (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Name TEXT NOT NULL,
    Description TEXT
);
```

### Tabela: Opportunities
```sql
CREATE TABLE Opportunities (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Title TEXT NOT NULL,
    Description TEXT,
    Date DATETIME,
    OrganizationId INTEGER NOT NULL,
    FOREIGN KEY (OrganizationId) REFERENCES Organizations(Id) ON DELETE CASCADE
);
```

### Tabela: Volunteers
```sql
CREATE TABLE Volunteers (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    FullName TEXT NOT NULL,
    Email TEXT NOT NULL
);
```

### Tabela: VolunteerOpportunities
```sql
CREATE TABLE VolunteerOpportunities (
    VolunteerId INTEGER NOT NULL,
    OpportunityId INTEGER NOT NULL,
    RegisteredAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    PRIMARY KEY (VolunteerId, OpportunityId),
    FOREIGN KEY (VolunteerId) REFERENCES Volunteers(Id) ON DELETE CASCADE,
    FOREIGN KEY (OpportunityId) REFERENCES Opportunities(Id) ON DELETE CASCADE
);
```

## Verificar o Banco de Dados

### Usando Visual Studio
1. Abra o `SQL Server Object Explorer` (View > SQL Server Object Explorer)
2. Clique em "Add SQL Server"
3. Conecte ao banco de dados local SQLite
4. Navegue até o arquivo `gestaovoluntariado.db`

### Usando SQLite Browser
1. Baixe o SQLite Browser: https://sqlitebrowser.org/
2. Abra o arquivo `gestaovoluntariado.db`
3. Visualize as tabelas e dados

## Resetar o Banco de Dados

Se precisar resetar o banco de dados:

### Opção 1: Deletar o arquivo
```bash
# No diretório do projeto
del gestaovoluntariado.db
```

Ao executar a aplicação novamente, um novo banco será criado.

### Opção 2: Usar Entity Framework CLI
```bash
dotnet ef database drop
dotnet ef database update
```

## Troubleshooting

### Problema: "No database provider has been configured"
**Solução:** Verifique se o `Program.cs` contém a configuração do SQLite:
```csharp
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));
```

### Problema: "The database file is locked"
**Solução:** Feche a aplicação e qualquer outro programa que esteja acessando o arquivo `gestaovoluntariado.db`

### Problema: Migrations não aparecem
**Solução:** Certifique-se de que o Entity Framework Tools está instalado:
```bash
dotnet tool install --global dotnet-ef
```

## Próximas Etapas

Após configurar o banco de dados:
1. Execute a aplicação (F5)
2. Crie uma organização em `/Organizations/Create`
3. Crie uma oportunidade em `/Opportunities/Create`
4. Inscreva-se em uma oportunidade em `/Opportunities/Details/{id}`

## Referências

- [Entity Framework Core Documentation](https://docs.microsoft.com/en-us/ef/core/)
- [ASP.NET Core MVC Documentation](https://docs.microsoft.com/en-us/aspnet/core/mvc/overview)
- [SQLite Documentation](https://www.sqlite.org/docs.html)
