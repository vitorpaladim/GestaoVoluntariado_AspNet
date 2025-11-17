# Gestão de Voluntariado - ASP.NET Core MVC

Uma aplicação ASP.NET Core MVC totalmente funcional para gerenciar organizações, oportunidades de voluntariado e voluntários. Este é um MVP simples que demonstra os principais conceitos de ASP.NET Core, Entity Framework Core e SQLite.

## Tecnologias Utilizadas

- **ASP.NET Core 8.0** - Framework web moderno
- **Entity Framework Core 8.0** - ORM para acesso a dados
- **SQLite** - Banco de dados local (arquivo `gestaovoluntariado.db`)
- **Razor Views** - Engine de templates
- **Cookie Authentication** - Autenticação simples por cookie
- **Bootstrap 5** - Framework CSS para UI responsiva

## Estrutura do Projeto

```
GestaoVoluntariado/
├── Controllers/              # Controladores MVC
│   ├── HomeController.cs
│   ├── OrganizationsController.cs
│   ├── OpportunitiesController.cs
│   └── AccountController.cs
├── Models/                   # Modelos de dados
│   ├── Organization.cs
│   ├── Opportunity.cs
│   ├── Volunteer.cs
│   └── VolunteerOpportunity.cs
├── Data/                     # Contexto do banco de dados
│   └── ApplicationDbContext.cs
├── Views/                    # Razor Views
│   ├── Home/
│   ├── Organizations/
│   ├── Opportunities/
│   ├── Account/
│   └── Shared/
├── wwwroot/                  # Arquivos estáticos (CSS, JS, imagens)
├── Program.cs                # Configuração da aplicação
├── appsettings.json          # Configurações
└── GestaoVoluntariado.csproj # Arquivo do projeto
```

## Funcionalidades Implementadas

### 1. Organizações
- ✅ Criar organização
- ✅ Editar organização
- ✅ Listar organizações

### 2. Oportunidades de Voluntariado
- ✅ Criar oportunidade vinculada a uma organização
- ✅ Listar oportunidades
- ✅ Ver detalhes da oportunidade

### 3. Voluntários
- ✅ Inscrição em oportunidade
- ✅ Criação automática de voluntário se não existir
- ✅ Listagem de voluntários inscritos

### 4. Autenticação
- ✅ Login simples por email
- ✅ Logout
- ✅ Autenticação por cookie

## Modelos de Dados

### Organization
```csharp
- Id (int, PK)
- Name (string, required)
- Description (string)
```

### Opportunity
```csharp
- Id (int, PK)
- Title (string, required)
- Description (string)
- Date (DateTime)
- OrganizationId (int, FK)
```

### Volunteer
```csharp
- Id (int, PK)
- FullName (string, required)
- Email (string, required)
```

### VolunteerOpportunity (Many-to-Many)
```csharp
- VolunteerId (int, FK, PK)
- OpportunityId (int, FK, PK)
- RegisteredAt (DateTime)
```

## Como Executar

### Pré-requisitos
- Visual Studio 2022 ou superior
- .NET 8.0 SDK instalado
- Git (opcional)

### Passos para Execução

1. **Abrir o projeto no Visual Studio**
   - Abra o Visual Studio
   - Clique em "Open a project or solution"
   - Navegue até a pasta `GestaoVoluntariado_AspNet`
   - Selecione o arquivo `GestaoVoluntariado.csproj`

2. **Restaurar dependências**
   - O Visual Studio restaurará automaticamente os pacotes NuGet
   - Ou execute no Package Manager Console:
     ```
     dotnet restore
     ```

3. **Criar o banco de dados**
   - O banco de dados SQLite será criado automaticamente na primeira execução
   - O arquivo `gestaovoluntariado.db` será gerado na raiz do projeto

4. **Executar a aplicação**
   - Pressione **F5** ou clique em "Start Debugging"
   - A aplicação abrirá no navegador padrão em `https://localhost:5001`

## Fluxos de Uso

### Fluxo 1: Criar Organização
1. Acesse `/Organizations/Create`
2. Preencha "Nome" e "Descrição"
3. Clique em "Criar"
4. Será redirecionado para a lista de organizações

### Fluxo 2: Criar Oportunidade
1. Acesse `/Opportunities/Create`
2. Selecione uma organização
3. Preencha "Título", "Descrição" e "Data"
4. Clique em "Criar"
5. Será redirecionado para a lista de oportunidades

### Fluxo 3: Inscrição do Voluntário
1. Acesse `/Opportunities/Details/{id}`
2. Veja as informações da oportunidade
3. Preencha "Nome Completo" e "Email"
4. Clique em "Inscrever-se"
5. Se o voluntário não existir, será criado automaticamente
6. O voluntário será registrado na oportunidade
7. A lista de voluntários inscritos será atualizada

### Fluxo 4: Login/Logout
1. Clique em "Login" no menu superior
2. Digite um email qualquer
3. Clique em "Entrar"
4. Será redirecionado para a página inicial
5. Para fazer logout, clique em "Logout" no menu

## Comandos Úteis

### Criar Migrations (se necessário)
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### Remover Banco de Dados
```bash
dotnet ef database drop
```

### Executar em modo Release
```bash
dotnet run --configuration Release
```

## Notas Importantes

- O banco de dados SQLite é um arquivo local (`gestaovoluntariado.db`) que será criado automaticamente
- A autenticação é simples e baseada em cookie - não há validação de senha
- O projeto usa Bootstrap 5 para styling básico
- Todas as views utilizam Razor syntax
- O projeto está configurado para usar HTTPS em desenvolvimento

## Estrutura de Pastas Criadas

```
GestaoVoluntariado_AspNet/
├── Controllers/
├── Models/
├── Data/
├── Views/
│   ├── Account/
│   ├── Home/
│   ├── Organizations/
│   ├── Opportunities/
│   └── Shared/
├── wwwroot/
│   ├── css/
│   └── js/
├── Program.cs
├── appsettings.json
├── appsettings.Development.json
├── GestaoVoluntariado.csproj
└── README.md
```

## Próximos Passos (Melhorias Futuras)

- Adicionar validação mais robusta
- Implementar autenticação com senha
- Adicionar paginação nas listas
- Implementar soft delete
- Adicionar testes unitários
- Melhorar o design da UI
- Adicionar filtros e busca
- Implementar notificações por email

## Suporte

Se encontrar algum problema:
1. Verifique se o .NET 8.0 SDK está instalado: `dotnet --version`
2. Limpe o cache de build: `dotnet clean`
3. Restaure os pacotes: `dotnet restore`
4. Recrie o banco de dados deletando `gestaovoluntariado.db`

## Licença

Este projeto é fornecido como um MVP educacional.
