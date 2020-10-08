namespace Web.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.AspNet.Identity.Owin;
    using Repository;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Web;
    using Web.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<Web.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(Web.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            //var UserManager = new UserManager<ApplicationUser,int>(new UserStore(context));
            //var PasswordHash = new PasswordHasher();
            //if (!context.Users.Any(u => u.UserName == "admin@admin.net"))
            //{
            //    var user = new ApplicationUser
            //    {
            //        UserName = "admin@ecom.com",
            //        Email = "admin@ecom.com",
            //        UserType = (int)UserType.Admin,
            //        PasswordHash = PasswordHash.HashPassword("123456")
            //    };

            //    UserManager.Create(user);
            //}
        }
    }
}
