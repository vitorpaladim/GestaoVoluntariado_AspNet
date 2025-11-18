using GestaoVoluntariado.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GestaoVoluntariado.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Organization> Organizations { get; set; } = null!;
        public DbSet<Opportunity> Opportunities { get; set; } = null!;
        public DbSet<Volunteer> Volunteers { get; set; } = null!;
        public DbSet<VolunteerOpportunity> VolunteerOpportunities { get; set; } = null!;
        public DbSet<ONG> ONGs { get; set; } = null!;
        public DbSet<Projeto> Projetos { get; set; } = null!;
        public DbSet<Voluntario> Voluntarios { get; set; } = null!;
        public DbSet<Candidatura> Candidaturas { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Voluntario>();
            modelBuilder.Entity<ONG>();

            // Configure Identity-derived relationships
            modelBuilder.Entity<Projeto>()
                .HasOne(p => p.ONG)
                .WithMany(o => o.Projetos)
                .HasForeignKey(p => p.OngId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Candidatura>()
                .HasOne(c => c.Projeto)
                .WithMany(p => p.Candidaturas)
                .HasForeignKey(c => c.ProjetoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Candidatura>()
                .HasOne(c => c.Voluntario)
                .WithMany(v => v.Candidaturas)
                .HasForeignKey(c => c.VoluntarioId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure the many-to-many relationship
            modelBuilder.Entity<VolunteerOpportunity>()
                .HasKey(vo => new { vo.VolunteerId, vo.OpportunityId });

            modelBuilder.Entity<VolunteerOpportunity>()
                .HasOne(vo => vo.Volunteer)
                .WithMany(v => v.VolunteerOpportunities)
                .HasForeignKey(vo => vo.VolunteerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<VolunteerOpportunity>()
                .HasOne(vo => vo.Opportunity)
                .WithMany(o => o.VolunteerOpportunities)
                .HasForeignKey(vo => vo.OpportunityId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure Organization to Opportunity relationship
            modelBuilder.Entity<Opportunity>()
                .HasOne(o => o.Organization)
                .WithMany(org => org.Opportunities)
                .HasForeignKey(o => o.OrganizationId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Volunteer>()
                .HasIndex(v => v.Email)
                .IsUnique();
        }
    }
}
