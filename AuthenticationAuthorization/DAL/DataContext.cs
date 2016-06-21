using System.Data.Entity;
using AuthenticationAuthorization.Models;

namespace AuthenticationAuthorization.DAL
{
    public class DataContext : DbContext
    {
        public DataContext()
            : base("DatabaseConn1") { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(u => u.Roles)// user can have many roles
                .WithMany(r => r.Users)// role contains multiple users in basic
                .Map(m =>
                {
                    m.ToTable("UserRoles");
                    m.MapLeftKey("UserId");
                    m.MapRightKey("RoleId");
                });
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
    }
}