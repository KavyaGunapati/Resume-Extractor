using Microsoft.EntityFrameworkCore;
using ResumeExtractorAPI.DataAccess.Entities;

namespace ResumeExtractorAPI.DataAccess.Context
{
    public  class AppDbContext : DbContext
    {
        public DbSet<ResumeResult> ResumeResults { get; set; }
        public DbSet<PersonalInfo> PersonalInfos { get; set; }  
        public DbSet<Education> Educations { get; set; }
        public DbSet<WorkExperience> WorkExperiences { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Certification> Certifications { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<Award> Awards { get; set; }
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ResumeResult>()
                .HasOne(r => r.PersonalInfo)
                .WithOne(pi => pi.ResumeResult)
                .HasForeignKey<PersonalInfo>(p => p.ResumeResultId);

            modelBuilder.Entity<ResumeResult>()
                .HasMany(r => r.Education)
                .WithOne(e => e.ResumeResult)
                .HasForeignKey(e => e.ResumeResultId);

            modelBuilder.Entity<ResumeResult>()
                .HasMany(r => r.WorkExperience)
                .WithOne(w => w.ResumeResult)
                .HasForeignKey(w => w.ResumeResultId);

            modelBuilder.Entity<ResumeResult>()
                .HasMany(r => r.Projects)
                .WithOne(p => p.ResumeResult)
                .HasForeignKey(p => p.ResumeResultId);

            modelBuilder.Entity<ResumeResult>()
                .HasMany(r => r.Certifications)
                .WithOne(c => c.ResumeResult)
                .HasForeignKey(c => c.ResumeResultId);

            modelBuilder.Entity<ResumeResult>()
                .HasMany(r => r.Languages)
                .WithOne(l => l.ResumeResult)
                .HasForeignKey(l => l.ResumeResultId);

            modelBuilder.Entity<ResumeResult>()
                .HasMany(r => r.Awards)
                .WithOne(a => a.ResumeResult)
                .HasForeignKey(a => a.ResumeResultId);
        }
    }

}