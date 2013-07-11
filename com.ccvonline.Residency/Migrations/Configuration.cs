namespace com.ccvonline.Residency.Migrations
{
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Reflection;

    // This is the Configuration class specifically for your Plugin.  
    // When doing "Add-Migration" and "Update-Database" operations, you might need to add the "-ConfigurationTypeName:Configuration" parameter 
    internal sealed class Configuration : DbMigrationsConfiguration<com.ccvonline.Residency.Data.ResidencyContext>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Configuration"/> class.
        /// </summary>
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            CodeGenerator = new Rock.Migrations.RockCSharpMigrationCodeGenerator<com.ccvonline.Residency.Data.ResidencyContext>(false);
        }

        /// <summary>
        /// Seeds the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        protected override void Seed(com.ccvonline.Residency.Data.ResidencyContext context)
        {
            //  This method will be called after migrating to the latest version.
        }
    }

    /*
    // If you update the Rock.dll often, this RockCoreConfiguration will help you do Update-Database for the Rock Core Tables
    // To do this, run "Update-Database -ConfigurationTypeName:RockCoreConfiguration" using Package Manager Console
    internal sealed class RockCoreConfiguration : DbMigrationsConfiguration<Rock.Data.RockContext>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RockCoreConfiguration"/> class.
        /// </summary>
        public RockCoreConfiguration()
        {
            this.MigrationsAssembly = typeof( Rock.Data.RockContext ).Assembly;
            this.MigrationsNamespace = "Rock.Migrations";
            this.ContextKey = "Rock.Migrations.Configuration";
        }
    }
     */ 
}
