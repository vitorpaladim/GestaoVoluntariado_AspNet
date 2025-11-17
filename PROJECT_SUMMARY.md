# Resumo do Projeto - Gestão de Voluntariado

## Visão Geral

Este é um projeto ASP.NET Core MVC completo e funcional chamado **"Gestão de Voluntariado"** que implementa um MVP (Minimum Viable Product) para gerenciar organizações, oportunidades de voluntariado e voluntários.

## Arquitetura

### Camadas
- **Presentation Layer**: Razor Views e Controllers
- **Business Logic**: Controllers e Models
- **Data Access Layer**: Entity Framework Core com ApplicationDbContext
- **Database**: SQLite (arquivo local)

### Padrões Utilizados
- **MVC Pattern**: Separação clara entre Models, Views e Controllers
- **Repository Pattern**: Implícito através do DbContext do EF Core
- **Dependency Injection**: Configurado no Program.cs

## Funcionalidades Principais

### 1. Gerenciamento de Organizações
- **Criar**: Formulário para criar nova organização
- **Editar**: Formulário para editar organização existente
- **Listar**: Tabela com todas as organizações

**Endpoints:**
- GET `/Organizations` - Listar
- GET `/Organizations/Create` - Formulário de criação
- POST `/Organizations/Create` - Salvar nova
- GET `/Organizations/Edit/{id}` - Formulário de edição
- POST `/Organizations/Edit/{id}` - Salvar edição

### 2. Gerenciamento de Oportunidades
- **Criar**: Formulário para criar oportunidade vinculada a organização
- **Listar**: Tabela com todas as oportunidades
- **Detalhes**: Página com informações completas e voluntários inscritos

**Endpoints:**
- GET `/Opportunities` - Listar
- GET `/Opportunities/Create` - Formulário de criação
- POST `/Opportunities/Create` - Salvar nova
- GET `/Opportunities/Details/{id}` - Ver detalhes
- POST `/Opportunities/Register` - Inscrever voluntário

### 3. Gerenciamento de Voluntários
- **Inscrição Automática**: Voluntário é criado automaticamente se não existir
- **Listagem**: Voluntários inscritos aparecem na página de detalhes da oportunidade
- **Relacionamento**: Muitos-para-muitos com oportunidades

### 4. Autenticação
- **Login**: Formulário simples com email
- **Logout**: Botão no menu de navegação
- **Sessão**: Baseada em cookie
- **Proteção**: CSRF token em todos os formulários

**Endpoints:**
- GET `/Account/Login` - Formulário de login
- POST `/Account/Login` - Processar login
- GET `/Account/Logout` - Fazer logout

## Estrutura de Arquivos

```
GestaoVoluntariado_AspNet/
│
├── Controllers/
│   ├── HomeController.cs           # Página inicial
│   ├── OrganizationsController.cs  # Gerenciar organizações
│   ├── OpportunitiesController.cs  # Gerenciar oportunidades
│   └── AccountController.cs        # Autenticação
│
├── Models/
│   ├── Organization.cs             # Modelo de organização
│   ├── Opportunity.cs              # Modelo de oportunidade
│   ├── Volunteer.cs                # Modelo de voluntário
│   └── VolunteerOpportunity.cs     # Modelo de relacionamento
│
├── Data/
│   └── ApplicationDbContext.cs     # Contexto do EF Core
│
├── Views/
│   ├── Home/
│   │   └── Index.cshtml            # Página inicial
│   ├── Organizations/
│   │   ├── Index.cshtml            # Lista de organizações
│   │   ├── Create.cshtml           # Criar organização
│   │   └── Edit.cshtml             # Editar organização
│   ├── Opportunities/
│   │   ├── Index.cshtml            # Lista de oportunidades
│   │   ├── Create.cshtml           # Criar oportunidade
│   │   └── Details.cshtml          # Detalhes e inscrição
│   ├── Account/
│   │   └── Login.cshtml            # Formulário de login
│   └── Shared/
│       ├── _Layout.cshtml          # Layout principal
│       ├── _ValidationScriptsPartial.cshtml
│       ├── _ViewImports.cshtml
│       └── _ViewStart.cshtml
│
├── wwwroot/
│   ├── css/
│   │   └── site.css                # Estilos customizados
│   └── js/
│       └── site.js                 # JavaScript customizado
│
├── Program.cs                      # Configuração da aplicação
├── appsettings.json                # Configurações
├── appsettings.Development.json    # Configurações de desenvolvimento
├── GestaoVoluntariado.csproj       # Arquivo do projeto
├── README.md                       # Documentação principal
├── SETUP_INSTRUCTIONS.md           # Instruções de setup
└── PROJECT_SUMMARY.md              # Este arquivo
```

## Dependências (NuGet Packages)

```xml
<PackageReference Include="Microsoft.EntityFrameworkCore" Version="8.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Sqlite" Version="8.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="8.0.0" />
```

## Configuração do Banco de Dados

### Connection String
```json
"ConnectionStrings": {
  "DefaultConnection": "Data Source=gestaovoluntariado.db"
}
```

### Criação Automática
O banco de dados é criado automaticamente na primeira execução através do código no `Program.cs`:
```csharp
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.EnsureCreated();
}
```

## Fluxo de Dados

### Fluxo 1: Criar Organização
```
User → GET /Organizations/Create
     → Preenche formulário
     → POST /Organizations/Create
     → Salva no banco (EF Core)
     → Redireciona para /Organizations
```

### Fluxo 2: Criar Oportunidade
```
User → GET /Opportunities/Create
     → Seleciona organização
     → Preenche formulário
     → POST /Opportunities/Create
     → Salva no banco com FK para Organization
     → Redireciona para /Opportunities
```

### Fluxo 3: Inscrição de Voluntário
```
User → GET /Opportunities/Details/{id}
     → Vê detalhes da oportunidade
     → Preenche formulário (Nome + Email)
     → POST /Opportunities/Register
     → Verifica se voluntário existe
     → Se não existe → Cria novo Volunteer
     → Cria registro em VolunteerOpportunity
     → Redireciona para /Opportunities/Details/{id}
     → Lista atualiza com novo voluntário
```

## Relacionamentos de Dados

### Organization → Opportunity (1:N)
- Uma organização pode ter muitas oportunidades
- Foreign Key: `Opportunity.OrganizationId`
- Cascade Delete: Deletar organização deleta oportunidades

### Opportunity ↔ Volunteer (N:N)
- Uma oportunidade pode ter muitos voluntários
- Um voluntário pode estar em muitas oportunidades
- Tabela de Junção: `VolunteerOpportunity`
- Cascade Delete: Deletar voluntário ou oportunidade remove o registro de junção

## Segurança

### Implementado
- ✅ CSRF Protection (ValidateAntiForgeryToken)
- ✅ Cookie Authentication
- ✅ HTTPS em desenvolvimento
- ✅ Model Validation

### Não Implementado (Para MVP)
- ❌ Autenticação com senha
- ❌ Autorização baseada em roles
- ❌ Encriptação de dados sensíveis
- ❌ Rate limiting
- ❌ SQL Injection prevention (EF Core previne automaticamente)

## Performance

- SQLite é adequado para MVP e desenvolvimento local
- Índices automáticos em chaves primárias e estrangeiras
- Lazy loading desabilitado (eager loading com Include)
- Sem paginação (adequado para volumes pequenos)

## Escalabilidade Futura

Para escalar para produção:
1. Migrar de SQLite para SQL Server ou PostgreSQL
2. Adicionar paginação
3. Implementar caching
4. Adicionar índices customizados
5. Implementar autenticação robusta
6. Adicionar logging e monitoramento

## Como Usar

### Executar
```bash
# Visual Studio
Pressione F5

# Ou via CLI
dotnet run
```

### Acessar
- URL: `https://localhost:5001`
- Página inicial: `/`
- Organizações: `/Organizations`
- Oportunidades: `/Opportunities`
- Login: `/Account/Login`

## Testes Manuais

### Teste 1: Criar Organização
1. Acesse `/Organizations/Create`
2. Preencha "Nome" = "ONG Ajuda"
3. Preencha "Descrição" = "Organização de ajuda social"
4. Clique em "Criar"
5. Verifique se aparece em `/Organizations`

### Teste 2: Criar Oportunidade
1. Acesse `/Opportunities/Create`
2. Selecione "ONG Ajuda"
3. Preencha "Título" = "Voluntário para limpeza"
4. Preencha "Descrição" = "Ajudar na limpeza da comunidade"
5. Selecione uma data
6. Clique em "Criar"
7. Verifique se aparece em `/Opportunities`

### Teste 3: Inscrever Voluntário
1. Acesse `/Opportunities/Details/1`
2. Preencha "Nome Completo" = "João Silva"
3. Preencha "Email" = "joao@email.com"
4. Clique em "Inscrever-se"
5. Verifique se "João Silva" aparece na lista de voluntários

## Próximas Melhorias

1. **UI/UX**: Melhorar design com Bootstrap customizado
2. **Validação**: Adicionar validações mais robustas
3. **Autenticação**: Implementar login com senha
4. **Autorização**: Adicionar roles (Admin, Voluntário)
5. **Funcionalidades**: Editar/deletar oportunidades, cancelar inscrição
6. **Testes**: Adicionar testes unitários e de integração
7. **Documentação**: Adicionar comentários XML
8. **Performance**: Adicionar paginação e filtros

## Conclusão

Este projeto é um MVP completo e funcional que demonstra os principais conceitos de ASP.NET Core MVC com Entity Framework Core. Está pronto para ser executado no Visual Studio sem modificações adicionais.
