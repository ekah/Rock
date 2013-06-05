namespace com.ccvonline.Residency.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<com.ccvonline.Residency.Data.ResidencyContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            CodeGenerator = new Rock.Migrations.RockCSharpMigrationCodeGenerator<com.ccvonline.Residency.Data.ResidencyContext>();
        }

        protected override void Seed(com.ccvonline.Residency.Data.ResidencyContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
