# Quick Start - Gestão de Voluntariado

## 🚀 Iniciar em 3 Passos

### 1. Abrir no Visual Studio
- Abra o **Visual Studio 2022**
- Clique em **File > Open > Project/Solution**
- Navegue até a pasta `GestaoVoluntariado_AspNet`
- Selecione o arquivo `GestaoVoluntariado.csproj`
- Clique em **Open**

### 2. Restaurar Dependências (Automático)
- O Visual Studio restaurará automaticamente os pacotes NuGet
- Aguarde até ver "Ready" na barra de status

### 3. Executar a Aplicação
- Pressione **F5** ou clique em **Start Debugging**
- O navegador abrirá automaticamente em `https://localhost:5001`
- O banco de dados SQLite será criado automaticamente

## ✅ Verificar se Tudo Está Funcionando

### Página Inicial
- Você deve ver a página inicial com 3 cards (Organizações, Oportunidades, Inscrever-se)

### Criar Organização
1. Clique em "Ir para Organizações" ou acesse `/Organizations`
2. Clique em "Criar Nova Organização"
3. Preencha:
   - **Nome**: "ONG Exemplo"
   - **Descrição**: "Uma organização de exemplo"
4. Clique em "Criar"
5. Você deve ver a organização na lista

### Criar Oportunidade
1. Acesse `/Opportunities` ou clique em "Ir para Oportunidades"
2. Clique em "Criar Nova Oportunidade"
3. Preencha:
   - **Título**: "Voluntário para Limpeza"
   - **Descrição**: "Ajude na limpeza da comunidade"
   - **Data**: Selecione uma data futura
   - **Organização**: Selecione "ONG Exemplo"
4. Clique em "Criar"
5. Você deve ver a oportunidade na lista

### Inscrever Voluntário
1. Na página de oportunidades, clique em "Detalhes" da oportunidade que criou
2. Preencha o formulário de inscrição:
   - **Nome Completo**: "João Silva"
   - **Email**: "joao@email.com"
3. Clique em "Inscrever-se"
4. Você deve ver "João Silva" na lista de voluntários inscritos

## 📁 Estrutura do Projeto

```
GestaoVoluntariado_AspNet/
├── Controllers/          ← Lógica das páginas
├── Models/              ← Estrutura dos dados
├── Views/               ← Páginas HTML (Razor)
├── Data/                ← Banco de dados
├── wwwroot/             ← CSS, JS, imagens
├── Program.cs           ← Configuração principal
└── appsettings.json     ← Configurações
```

## 🔗 URLs Principais

| Página | URL |
|--------|-----|
| Home | `/` |
| Organizações | `/Organizations` |
| Criar Organização | `/Organizations/Create` |
| Editar Organização | `/Organizations/Edit/{id}` |
| Oportunidades | `/Opportunities` |
| Criar Oportunidade | `/Opportunities/Create` |
| Detalhes Oportunidade | `/Opportunities/Details/{id}` |
| Login | `/Account/Login` |
| Logout | `/Account/Logout` |

## 🛠️ Troubleshooting

### Problema: Porta 5001 já está em uso
**Solução**: Mude a porta em `Properties/launchSettings.json`

### Problema: "The database file is locked"
**Solução**: Feche a aplicação e tente novamente

### Problema: Erro ao restaurar pacotes
**Solução**: 
1. Clique em **Tools > NuGet Package Manager > Package Manager Console**
2. Digite: `dotnet restore`
3. Pressione Enter

### Problema: Banco de dados não foi criado
**Solução**: O banco é criado automaticamente. Se não funcionar:
1. Feche a aplicação
2. Delete o arquivo `gestaovoluntariado.db` (se existir)
3. Execute novamente (F5)

## 📝 Próximas Ações

Depois de testar:
1. Explore o código em `Controllers/` para entender a lógica
2. Modifique as `Views/` para customizar o design
3. Adicione novos campos aos `Models/`
4. Implemente novas funcionalidades

## 📚 Documentação Completa

- **README.md** - Documentação completa do projeto
- **PROJECT_SUMMARY.md** - Resumo técnico
- **SETUP_INSTRUCTIONS.md** - Instruções de setup do banco de dados

## ⚡ Dicas Rápidas

- **Recarregar página**: Pressione F5 no navegador
- **Parar aplicação**: Pressione Shift+F5 ou clique no botão Stop
- **Modo Debug**: Coloque breakpoints clicando na margem esquerda do código
- **Ver banco de dados**: Use SQLite Browser (https://sqlitebrowser.org/)

---

**Pronto para começar? Pressione F5 agora! 🎉**
