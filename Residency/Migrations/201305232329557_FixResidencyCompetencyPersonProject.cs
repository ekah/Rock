namespace com.ccvonline.Residency.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixResidencyCompetencyPersonProject : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo._com_ccvonline_ResidencyCompetencyPersonProject", "ResidencyProjectId", c => c.Int(nullable: false));
            CreateIndex("dbo._com_ccvonline_ResidencyCompetencyPersonProject", "ResidencyProjectId");
            AddForeignKey("dbo._com_ccvonline_ResidencyCompetencyPersonProject", "ResidencyProjectId", "dbo._com_ccvonline_ResidencyProject", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo._com_ccvonline_ResidencyCompetencyPersonProject", "ResidencyProjectId", "dbo._com_ccvonline_ResidencyProject");
            DropIndex("dbo._com_ccvonline_ResidencyCompetencyPersonProject", new[] { "ResidencyProjectId" });
            DropColumn("dbo._com_ccvonline_ResidencyCompetencyPersonProject", "ResidencyProjectId");
        }
    }
}
