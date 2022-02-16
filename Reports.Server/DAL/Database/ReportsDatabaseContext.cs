using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Reports.Server.Domain.Entities.Employee;
using Reports.Server.Domain.Entities.Report;
using Reports.Server.Domain.Entities.Task;

namespace Reports.Server.DAL.Database
{
    public sealed class ReportsDatabaseContext : DbContext
    {
        public ReportsDatabaseContext()
        {
            Database.EnsureCreated();
        }

        public ReportsDatabaseContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=reports4;Username=postgres;Password=password");
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<Report> Reports { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Employee>().HasKey(employee => employee.Id);
            modelBuilder.Entity<Employee>().Property(employee => employee.Name);
            modelBuilder.Entity<Employee>().Property(employee => employee.State)
                .HasConversion(new EnumToStringConverter<EmployeeState>());
            modelBuilder.Entity<Employee>().Property(employee => employee.CreationTime);
            modelBuilder.Entity<Employee>().HasOne(employee => employee.Report);
            modelBuilder.Entity<Employee>().Property(employee => employee.SuperiorId);

            modelBuilder.Entity<Task>().HasKey(task => task.Id);
            modelBuilder.Entity<Task>().Property(task => task.Description);
            modelBuilder.Entity<Task>().Property(task => task.State)
                .HasConversion(new EnumToStringConverter<TaskState>());
            modelBuilder.Entity<Task>().Property(task => task.CreationTime);
            modelBuilder.Entity<Task>().OwnsMany(task => task.Comments);
            modelBuilder.Entity<Task>().OwnsMany(task => task.UpdateHistory);

            modelBuilder.Entity<Report>().HasKey(report => report.Id);
            modelBuilder.Entity<Report>().Property(report => report.Description);
            modelBuilder.Entity<Report>().Property(report => report.CreationTime);
            modelBuilder.Entity<Report>().Property(report => report.FinishTime);
            modelBuilder.Entity<Report>().Property(report => report.State)
                .HasConversion(new EnumToStringConverter<ReportState>());
            modelBuilder.Entity<Report>().Property(report => report.AuthorId);
            modelBuilder.Entity<Report>().HasMany(report => report.Tasks);
        }
    }
}