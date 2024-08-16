using Microsoft.EntityFrameworkCore;
using MyProjectJWT.Models;

namespace MyProjectJWT.Context
{
    public class JwtContext : DbContext
    {
        public JwtContext(DbContextOptions<JwtContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Child> Children { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Vaccines> Vaccines { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuring the relationship between User and UserRoles (One-to-Many)
            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuring the relationship between Role and UserRoles (One-to-Many)
            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuring the relationship between Child and User (One-to-Many, Unidirectional)
            modelBuilder.Entity<Child>()
                .HasOne<User>()
                .WithMany(u => u.Children)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuring the relationship between Appointment and Child (Many-to-One)
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Child)
                .WithMany()
                .HasForeignKey(a => a.ChildId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuring the relationship between Appointment and User (Many-to-One)
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.User)
                .WithMany()
                .HasForeignKey(a => a.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuring the relationship between Appointment and Vaccine (Many-to-One)
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Vaccine)
                .WithMany()
                .HasForeignKey(a => a.VaccineId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
    }
}
