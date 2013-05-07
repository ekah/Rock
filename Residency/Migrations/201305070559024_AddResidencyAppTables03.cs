namespace com.ccvonline.Residency.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddResidencyAppTables03 : DbMigration
    {
        /// <summary>
        /// Operations to be performed during the upgrade process.
        /// </summary>
        public override void Up()
        {
            DropForeignKey("dbo._com_ccvonline_ResidencyCompetencyPersonProject", "ResidencyCompetencyPersonId", "dbo.Person");
            DropIndex("dbo._com_ccvonline_ResidencyCompetencyPersonProject", new[] { "ResidencyCompetencyPersonId" });
            CreateIndex("dbo._com_ccvonline_ResidencyCompetencyPersonProject", "ResidencyCompetencyPersonId");
            AddForeignKey("dbo._com_ccvonline_ResidencyCompetencyPersonProject", "ResidencyCompetencyPersonId", "dbo._com_ccvonline_ResidencyCompetencyPerson", "Id");
        }

        /// <summary>
        /// Operations to be performed during the downgrade process.
        /// </summary>
        public override void Down()
        {
            DropForeignKey("dbo._com_ccvonline_ResidencyCompetencyPersonProject", "ResidencyCompetencyPersonId", "dbo._com_ccvonline_ResidencyCompetencyPerson");
            DropIndex("dbo._com_ccvonline_ResidencyCompetencyPersonProject", new[] { "ResidencyCompetencyPersonId" });
            CreateIndex("dbo._com_ccvonline_ResidencyCompetencyPersonProject", "ResidencyCompetencyPersonId");
            AddForeignKey("dbo._com_ccvonline_ResidencyCompetencyPersonProject", "ResidencyCompetencyPersonId", "dbo.Person", "Id");
        }
    }
}
