using Microsoft.EntityFrameworkCore;
using GestaoVoluntariado.Models;

namespace GestaoVoluntariado.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Organization> Organizations { get; set; } = null!;
        public DbSet<Opportunity> Opportunities { get; set; } = null!;
        public DbSet<Volunteer> Volunteers { get; set; } = null!;
        public DbSet<VolunteerOpportunity> VolunteerOpportunities { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

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
