
using Application.Common.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoDomain;

namespace Application.Infrastructure.Common.Percistance
{
    public class AppDBContext : DbContext
    {
        public DbSet<TodoItem> ToDoItems { get; set; }

        public DbSet<AppUser> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public AppDBContext(DbContextOptions<AppDBContext> dbContextOptions) : base(dbContextOptions)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TodoItem>().HasKey(c => c.Id);
            modelBuilder.Entity<TodoItem>().Property(b => b.Label).HasMaxLength(100).IsRequired();

            modelBuilder.Entity<AppUser>().HasKey(u => u.Id);
            modelBuilder.Entity<AppUser>().Property(b => b.Login).HasMaxLength(50).IsRequired();
            modelBuilder.Entity<AppUser>().HasIndex(u => u.Login).IsUnique();

            modelBuilder.Entity<AppUser>().HasMany(e => e.AppUserAppRoles);
            modelBuilder.Entity<AppUserRole>().HasMany(e => e.AppUserAppRole);

            modelBuilder.Entity<AppUserAppRole>().HasOne(e => e.Role);
            modelBuilder.Entity<AppUserAppRole>().HasOne(e => e.User);

            modelBuilder.Entity<TodoItem>()
                .HasOne(v => v.User);


            modelBuilder.Entity<RefreshToken>().HasKey(c => c.Id);
            modelBuilder.Entity<RefreshToken>().Property(e => e.Id).HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<RefreshToken>().HasOne(c => c.User).WithMany().HasForeignKey(e => e.UserId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
