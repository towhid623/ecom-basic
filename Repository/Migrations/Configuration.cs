namespace Repository.Migrations
{
    using Entities.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ProjectDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(ProjectDbContext context)
        {
            context.FndFlexValueSet.AddOrUpdate(a => a.FlexValueSetShortName,
                new FndFlexValueSet {FlexValueSetName = "Unit", FlexValueSetShortName = "unit" },
                new FndFlexValueSet {FlexValueSetName = "Brand", FlexValueSetShortName = "brand" }
            );
        }
    }
}
