namespace com.ccvonline.Residency.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddResidencyAppTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo._com_ccvonline_ResidencyPeriod",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StartDate = c.DateTimeOffset(),
                        EndDate = c.DateTimeOffset(),
                        Name = c.String(nullable: false, maxLength: 100),
                        Description = c.String(),
                        Guid = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id);

            // make sure to create index for Guid for new tables
            CreateIndex( "dbo._com_ccvonline_ResidencyPeriod", "Guid", true );
            
            CreateTable(
                "dbo._com_ccvonline_ResidencyTrack",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ResidencyPeriodId = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100),
                        Description = c.String(),
                        Guid = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo._com_ccvonline_ResidencyPeriod", t => t.ResidencyPeriodId, cascadeDelete: true)
                .Index(t => t.ResidencyPeriodId);

            // make sure to create index for Guid for new tables
            CreateIndex( "dbo._com_ccvonline_ResidencyTrack", "Guid", true );
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo._com_ccvonline_ResidencyTrack", "ResidencyPeriodId", "dbo._com_ccvonline_ResidencyPeriod");
            DropIndex("dbo._com_ccvonline_ResidencyTrack", new[] { "ResidencyPeriodId" });
            DropTable("dbo._com_ccvonline_ResidencyTrack");
            DropTable("dbo._com_ccvonline_ResidencyPeriod");
        }
    }
}
