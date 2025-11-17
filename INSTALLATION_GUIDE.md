# Guia Completo de Instalação - Gestão de Voluntariado

## 📋 Pré-requisitos

Antes de começar, certifique-se de ter instalado:

### 1. Visual Studio 2022
- **Download**: https://visualstudio.microsoft.com/vs/
- **Versão mínima**: Community (gratuita) ou superior
- **Componentes necessários**:
  - ASP.NET and web development
  - .NET desktop development

### 2. .NET 8.0 SDK
- **Download**: https://dotnet.microsoft.com/download/dotnet/8.0
- **Verificar instalação**:
  ```bash
  dotnet --version
  ```
  Deve retornar algo como: `8.0.x`

### 3. Git (Opcional)
- **Download**: https://git-scm.com/
- Útil para controle de versão

## 🔧 Instalação do Visual Studio 2022

### Passo 1: Baixar o Instalador
1. Acesse https://visualstudio.microsoft.com/vs/
2. Clique em "Download Visual Studio"
3. Selecione "Community" (gratuito)
4. Execute o instalador

### Passo 2: Selecionar Componentes
Na tela de seleção de componentes, marque:
- ✅ **ASP.NET and web development**
- ✅ **.NET desktop development**
- ✅ **Data storage and processing** (opcional, para SQL Server)

### Passo 3: Concluir Instalação
1. Clique em "Install"
2. Aguarde a conclusão (pode levar 10-30 minutos)
3. Reinicie o computador se solicitado

## 📦 Instalação do .NET 8.0 SDK

### Passo 1: Baixar
1. Acesse https://dotnet.microsoft.com/download/dotnet/8.0
2. Selecione a versão para seu sistema operacional (Windows, macOS, Linux)
3. Baixe o **SDK** (não apenas o Runtime)

### Passo 2: Instalar
1. Execute o instalador
2. Siga as instruções na tela
3. Reinicie o computador

### Passo 3: Verificar Instalação
Abra o **Command Prompt** ou **PowerShell** e execute:
```bash
dotnet --version
```

Deve exibir: `8.0.x` ou superior

## 🚀 Configurar o Projeto

### Opção 1: Abrir no Visual Studio (Recomendado)

#### Passo 1: Extrair o Projeto
1. Extraia o arquivo `GestaoVoluntariado_AspNet.zip`
2. Navegue até a pasta `GestaoVoluntariado_AspNet`

#### Passo 2: Abrir no Visual Studio
1. Abra o **Visual Studio 2022**
2. Clique em **File > Open > Project/Solution**
3. Navegue até a pasta do projeto
4. Selecione `GestaoVoluntariado.csproj`
5. Clique em **Open**

#### Passo 3: Restaurar Dependências
- O Visual Studio restaurará automaticamente os pacotes NuGet
- Aguarde até ver "Ready" na barra de status inferior
- Se não restaurar automaticamente:
  - Clique em **Tools > NuGet Package Manager > Manage NuGet Packages for Solution**
  - Clique em **Restore**

### Opção 2: Abrir via Linha de Comando

#### Passo 1: Navegar até o Projeto
```bash
cd caminho\para\GestaoVoluntariado_AspNet
```

#### Passo 2: Restaurar Dependências
```bash
dotnet restore
```

#### Passo 3: Compilar o Projeto
```bash
dotnet build
```

#### Passo 4: Executar
```bash
dotnet run
```

A aplicação estará disponível em: `https://localhost:5001`

## 🗄️ Configuração do Banco de Dados

### Criação Automática
O banco de dados SQLite é criado **automaticamente** na primeira execução. Não é necessário fazer nada!

### Verificar Criação
Após executar a aplicação pela primeira vez:
1. Navegue até a pasta do projeto
2. Você verá um arquivo chamado `gestaovoluntariado.db`
3. Este é seu banco de dados SQLite

### Visualizar Dados (Opcional)
Para visualizar e gerenciar o banco de dados:

#### Opção 1: SQLite Browser
1. Baixe em: https://sqlitebrowser.org/
2. Abra o arquivo `gestaovoluntariado.db`
3. Explore as tabelas e dados

#### Opção 2: Visual Studio
1. Abra **View > SQL Server Object Explorer**
2. Clique em **Add SQL Server**
3. Conecte ao banco de dados local SQLite

## ✅ Verificar Instalação

### Passo 1: Executar a Aplicação
1. No Visual Studio, pressione **F5**
2. Ou via CLI: `dotnet run`

### Passo 2: Acessar a Página Inicial
- A aplicação deve abrir automaticamente em `https://localhost:5001`
- Você deve ver a página inicial com 3 cards

### Passo 3: Testar Funcionalidades

#### Teste 1: Criar Organização
1. Clique em "Ir para Organizações"
2. Clique em "Criar Nova Organização"
3. Preencha os campos
4. Clique em "Criar"

#### Teste 2: Criar Oportunidade
1. Clique em "Ir para Oportunidades"
2. Clique em "Criar Nova Oportunidade"
3. Selecione uma organização
4. Preencha os campos
5. Clique em "Criar"

#### Teste 3: Inscrever Voluntário
1. Clique em "Detalhes" de uma oportunidade
2. Preencha Nome e Email
3. Clique em "Inscrever-se"

## 🐛 Troubleshooting

### Problema 1: "The project file could not be loaded"
**Causa**: Arquivo .csproj corrompido ou versão .NET incorreta
**Solução**:
1. Verifique se tem .NET 8.0 instalado: `dotnet --version`
2. Feche o Visual Studio
3. Delete a pasta `bin` e `obj`
4. Abra novamente

### Problema 2: "Package restore failed"
**Causa**: Problema com NuGet
**Solução**:
1. Abra **Tools > NuGet Package Manager > Package Manager Console**
2. Execute: `dotnet restore`
3. Se persistir, limpe o cache: `dotnet nuget locals all --clear`

### Problema 3: "The database file is locked"
**Causa**: Aplicação ainda está rodando
**Solução**:
1. Feche a aplicação (Shift+F5)
2. Aguarde 5 segundos
3. Execute novamente

### Problema 4: "Port 5001 is already in use"
**Causa**: Outra aplicação está usando a porta
**Solução**:
1. Abra `Properties/launchSettings.json`
2. Mude `"applicationUrl": "https://localhost:5001"` para outra porta, ex: `5002`
3. Salve e execute novamente

### Problema 5: Banco de dados não foi criado
**Causa**: Erro na inicialização
**Solução**:
1. Abra **Package Manager Console**
2. Execute: `dotnet ef database drop`
3. Execute: `dotnet ef database update`
4. Ou simplesmente delete `gestaovoluntariado.db` e execute a aplicação novamente

## 📝 Arquivos Importantes

| Arquivo | Descrição |
|---------|-----------|
| `GestaoVoluntariado.csproj` | Arquivo do projeto |
| `Program.cs` | Configuração principal |
| `appsettings.json` | Configurações de produção |
| `appsettings.Development.json` | Configurações de desenvolvimento |
| `gestaovoluntariado.db` | Banco de dados SQLite (criado automaticamente) |

## 🎯 Próximos Passos

Depois de instalar com sucesso:

1. **Explorar o Código**
   - Abra `Controllers/` para entender a lógica
   - Abra `Views/` para ver as páginas
   - Abra `Models/` para ver a estrutura dos dados

2. **Customizar**
   - Modifique as views em `Views/`
   - Adicione novos campos aos modelos em `Models/`
   - Implemente novas funcionalidades

3. **Aprender Mais**
   - Leia `README.md` para documentação completa
   - Leia `PROJECT_SUMMARY.md` para visão técnica
   - Leia `QUICK_START.md` para começar rápido

## 📚 Recursos Úteis

- **ASP.NET Core Documentation**: https://docs.microsoft.com/aspnet/core/
- **Entity Framework Core**: https://docs.microsoft.com/ef/core/
- **Razor Syntax**: https://docs.microsoft.com/aspnet/core/mvc/views/razor
- **Bootstrap 5**: https://getbootstrap.com/docs/5.0/
- **SQLite**: https://www.sqlite.org/docs.html

## 🆘 Suporte

Se encontrar problemas:

1. Verifique se todos os pré-requisitos estão instalados
2. Leia o arquivo `README.md`
3. Consulte `SETUP_INSTRUCTIONS.md` para problemas com banco de dados
4. Verifique a documentação oficial dos links acima

---

**Pronto para começar? Siga os passos acima e execute F5! 🎉**
