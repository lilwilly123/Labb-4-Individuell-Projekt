using Microsoft.EntityFrameworkCore;
using SchoolApp.Models;

namespace SchoolApp;

public class SchoolContext : DbContext
{
    public DbSet<Student> Student => Set<Student>();
    public DbSet<Staff> Staff => Set<Staff>();
    public DbSet<Class> Class => Set<Class>();
    public DbSet<Course> Course => Set<Course>();
    public DbSet<Grade> Grade => Set<Grade>();
    public DbSet<GradeLastMonthView> GradesLastMonth => Set<GradeLastMonthView>();
    public DbSet<Department> Department { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(
                "Server=.;Database=Labb2;Trusted_Connection=True;TrustServerCertificate=True;");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Class>()
            .HasOne(c => c.HomeroomTeacher)
            .WithMany(t => t.HomeroomClasses)
            .HasForeignKey(c => c.HomeroomTeacherId);

        modelBuilder.Entity<Grade>()
            .HasOne(g => g.Teacher)
            .WithMany(t => t.GradesGiven)
            .HasForeignKey(g => g.TeacherId);

        modelBuilder.Entity<GradeLastMonthView>()
       .HasNoKey()
       .ToView("vw_GradesLastMonth");
    }
}
