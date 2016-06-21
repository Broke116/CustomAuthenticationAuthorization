using System.Collections.Generic;
using AuthenticationAuthorization.Models;

namespace AuthenticationAuthorization.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<DAL.DataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DAL.DataContext context)
        {
            Role role1 = new Role { RoleName = "Admin" };
            Role role2 = new Role { RoleName = "User" };

            User user1 = new User { Username = "admin", Email = "admin@mail.com", FirstName = "Admin", Password = "123456", IsActive = true, CreateDate = DateTime.UtcNow, Roles = new List<Role>() };
            User user2 = new User { Username = "user1", Email = "user1@mail.com", FirstName = "User1", Password = "123456", IsActive = true, CreateDate = DateTime.UtcNow, Roles = new List<Role>() };
            
            user1.Roles.Add(role1);
            user2.Roles.Add(role2);
            context.Users.Add(user1);
            context.Users.Add(user2);
        }
    }
}
