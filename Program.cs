using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using GestaoVoluntariado.Data;
using GestaoVoluntariado.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();

// Add Entity Framework Core with SQL Server
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
// Add Cookie Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/Login";
    });

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Create database and seed data on startup
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();



    // Seed Organizations
    if (!dbContext.Organizations.Any())
    {
        var organizations = new List<Organization>
        {
            new Organization
            {
                Name = "ONG Ajuda Social",
                Description = "Organização dedicada a ajudar comunidades carentes com alimentos e roupas."
            },
            new Organization
            {
                Name = "Proteção Animal",
                Description = "Trabalhamos no resgate e adoção de animais abandonados."
            },
            new Organization
            {
                Name = "Amigos do Meio Ambiente",
                Description = "Focamos em preservação ambiental e limpeza de áreas públicas."
            },
            new Organization
            {
                Name = "Educação Para Todos",
                Description = "Oferecemos aulas de reforço gratuitas para crianças e adultos."
            }
        };

        dbContext.Organizations.AddRange(organizations);
        dbContext.SaveChanges();

        Console.WriteLine("✅ 4 Organizações criadas com sucesso!");
    }

    // Seed Opportunities
    if (!dbContext.Opportunities.Any())
    {
        var opportunities = new List<Opportunity>
        {
            new Opportunity
            {
                Title = "Distribuição de Alimentos",
                Description = "Ajude na distribuição de cestas básicas para famílias necessitadas.",
                Date = DateTime.Now.AddDays(7),
                OrganizationId = 1
            },
            new Opportunity
            {
                Title = "Campanha de Agasalho",
                Description = "Arrecadação e distribuição de roupas de inverno.",
                Date = DateTime.Now.AddDays(14),
                OrganizationId = 1
            },
            new Opportunity
            {
                Title = "Resgate de Animais",
                Description = "Apoio no resgate e cuidado de animais abandonados.",
                Date = DateTime.Now.AddDays(5),
                OrganizationId = 2
            },
            new Opportunity
            {
                Title = "Feira de Adoção",
                Description = "Organização de feira de adoção de cães e gatos.",
                Date = DateTime.Now.AddDays(21),
                OrganizationId = 2
            },
            new Opportunity
            {
                Title = "Limpeza de Praia",
                Description = "Mutirão de limpeza na praia local.",
                Date = DateTime.Now.AddDays(10),
                OrganizationId = 3
            },
            new Opportunity
            {
                Title = "Plantio de Árvores",
                Description = "Reflorestamento de área degradada no parque municipal.",
                Date = DateTime.Now.AddDays(30),
                OrganizationId = 3
            },
            new Opportunity
            {
                Title = "Aulas de Reforço - Matemática",
                Description = "Dar aulas de reforço de matemática para crianças do ensino fundamental.",
                Date = DateTime.Now.AddDays(3),
                OrganizationId = 4
            },
            new Opportunity
            {
                Title = "Alfabetização de Adultos",
                Description = "Ajudar na alfabetização de adultos em comunidade carente.",
                Date = DateTime.Now.AddDays(15),
                OrganizationId = 4
            }
        };

        dbContext.Opportunities.AddRange(opportunities);
        dbContext.SaveChanges();

        Console.WriteLine("✅ 8 Oportunidades criadas com sucesso!");
    }

    // Seed Volunteers (opcional)
    if (!dbContext.Volunteers.Any())
    {
        var volunteers = new List<Volunteer>
        {
            new Volunteer
            {
                FullName = "João Silva",
                Email = "joao.silva@email.com"
            },
            new Volunteer
            {
                FullName = "Maria Santos",
                Email = "maria.santos@email.com"
            },
            new Volunteer
            {
                FullName = "Pedro Oliveira",
                Email = "pedro.oliveira@email.com"
            }
        };

        dbContext.Volunteers.AddRange(volunteers);
        dbContext.SaveChanges();

        Console.WriteLine("✅ 3 Voluntários criados com sucesso!");

        // Criar algumas inscrições de exemplo
        var registrations = new List<VolunteerOpportunity>
        {
            new VolunteerOpportunity
            {
                VolunteerId = 1,
                OpportunityId = 1,
                RegisteredAt = DateTime.Now.AddDays(-2)
            },
            new VolunteerOpportunity
            {
                VolunteerId = 1,
                OpportunityId = 5,
                RegisteredAt = DateTime.Now.AddDays(-1)
            },
            new VolunteerOpportunity
            {
                VolunteerId = 2,
                OpportunityId = 3,
                RegisteredAt = DateTime.Now.AddDays(-3)
            },
            new VolunteerOpportunity
            {
                VolunteerId = 3,
                OpportunityId = 7,
                RegisteredAt = DateTime.Now.AddDays(-1)
            }
        };

        dbContext.VolunteerOpportunities.AddRange(registrations);
        dbContext.SaveChanges();

        Console.WriteLine("✅ 4 Inscrições criadas com sucesso!");
    }

    Console.WriteLine("🎉 Banco de dados inicializado com sucesso!");
}

app.Run();