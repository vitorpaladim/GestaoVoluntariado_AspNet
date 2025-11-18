using GestaoVoluntariado.Data;
using GestaoVoluntariado.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews()
    .AddRazorRuntimeCompilation(); // <-- LINHA CORRIGIDA PARA FORÇAR A COMPILAÇÃO DAS VIEWS

// Add Entity Framework Core with SQL Server
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        options.Password.RequiredLength = 6;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireDigit = false;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/AccessDenied";
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

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await SeedDatabaseAsync(services);
}

app.Run();

static async Task SeedDatabaseAsync(IServiceProvider services)
{
    var dbContext = services.GetRequiredService<ApplicationDbContext>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();


    var roles = new[] { "Admin", "ONG", "Voluntario" };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    var adminEmail = "admin@gestaovoluntariado.com";
    var adminUser = await userManager.FindByEmailAsync(adminEmail);
    if (adminUser == null)
    {
        adminUser = new ApplicationUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(adminUser, "Admin@123");
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }

    if (!await dbContext.Organizations.AnyAsync())
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
        await dbContext.SaveChangesAsync();
    }

    if (!await dbContext.Opportunities.AnyAsync())
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
        await dbContext.SaveChangesAsync();
    }

    if (!await dbContext.Volunteers.AnyAsync())
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
        await dbContext.SaveChangesAsync();

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
        await dbContext.SaveChangesAsync();
    }
}