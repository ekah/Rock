namespace com.ccvonline.Residency.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddResidencyAppTables02 : DbMigration
    {
        /// <summary>
        /// Operations to be performed during the upgrade process.
        /// </summary>
        public override void Up()
        {
            DropForeignKey("dbo._com_ccvonline_ResidencyTrack", "ResidencyPeriodId", "dbo._com_ccvonline_ResidencyPeriod");
            DropIndex("dbo._com_ccvonline_ResidencyTrack", new[] { "ResidencyPeriodId" });
            CreateTable(
                "dbo._com_ccvonline_ResidencyCompetency",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ResidencyTrackId = c.Int(nullable: false),
                        TeacherOfRecordPersonId = c.Int(),
                        FacilitatorPersonId = c.Int(),
                        Goals = c.String(),
                        CreditHours = c.Int(),
                        SupervisionHours = c.Int(),
                        ImplementationHours = c.Int(),
                        Name = c.String(nullable: false, maxLength: 100),
                        Description = c.String(),
                        Guid = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo._com_ccvonline_ResidencyTrack", t => t.ResidencyTrackId)
                .ForeignKey("dbo.Person", t => t.TeacherOfRecordPersonId)
                .ForeignKey("dbo.Person", t => t.FacilitatorPersonId)
                .Index(t => t.ResidencyTrackId)
                .Index(t => t.TeacherOfRecordPersonId)
                .Index(t => t.FacilitatorPersonId);

            // make sure to create index for Guid for new tables
            CreateIndex( "dbo._com_ccvonline_ResidencyCompetency", "Guid", true );
            
            CreateTable(
                "dbo._com_ccvonline_ResidencyProject",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ResidencyCompetencyId = c.Int(nullable: false),
                        MinAssignmentCountDefault = c.Int(),
                        Name = c.String(nullable: false, maxLength: 100),
                        Description = c.String(),
                        Guid = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo._com_ccvonline_ResidencyCompetency", t => t.ResidencyCompetencyId)
                .Index(t => t.ResidencyCompetencyId);

            // make sure to create index for Guid for new tables
            CreateIndex( "dbo._com_ccvonline_ResidencyProject", "Guid", true );
            
            CreateTable(
                "dbo._com_ccvonline_ResidencyProjectPointOfAssessment",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ResidencyProjectId = c.Int(nullable: false),
                        AssessmentOrder = c.Int(nullable: false),
                        AssessmentText = c.String(nullable: false),
                        Guid = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo._com_ccvonline_ResidencyProject", t => t.ResidencyProjectId)
                .Index(t => t.ResidencyProjectId);

            // make sure to create index for Guid for new tables
            CreateIndex( "dbo._com_ccvonline_ResidencyProjectPointOfAssessment", "Guid", true );
            
            CreateTable(
                "dbo._com_ccvonline_ResidencyCompetencyPerson",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ResidencyCompetencyId = c.Int(nullable: false),
                        PersonId = c.Int(nullable: false),
                        Guid = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo._com_ccvonline_ResidencyCompetency", t => t.ResidencyCompetencyId)
                .ForeignKey("dbo.Person", t => t.PersonId)
                .Index(t => t.ResidencyCompetencyId)
                .Index(t => t.PersonId);

            // make sure to create index for Guid for new tables
            CreateIndex( "dbo._com_ccvonline_ResidencyCompetencyPerson", "Guid", true );
            
            CreateTable(
                "dbo._com_ccvonline_ResidencyCompetencyPersonProject",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ResidencyCompetencyPersonId = c.Int(nullable: false),
                        MinAssignmentCount = c.Int(),
                        Guid = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Person", t => t.ResidencyCompetencyPersonId)
                .Index(t => t.ResidencyCompetencyPersonId);

            // make sure to create index for Guid for new tables
            CreateIndex( "dbo._com_ccvonline_ResidencyCompetencyPersonProject", "Guid", true );
            
            CreateTable(
                "dbo._com_ccvonline_ResidencyCompetencyPersonProjectAssignment",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ResidencyCompetencyPersonProjectId = c.Int(nullable: false),
                        AssessorPersonId = c.Int(),
                        CompletedDateTime = c.DateTime(),
                        Guid = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo._com_ccvonline_ResidencyCompetencyPersonProject", t => t.ResidencyCompetencyPersonProjectId)
                .ForeignKey("dbo.Person", t => t.AssessorPersonId)
                .Index(t => t.ResidencyCompetencyPersonProjectId)
                .Index(t => t.AssessorPersonId);

            // make sure to create index for Guid for new tables
            CreateIndex( "dbo._com_ccvonline_ResidencyCompetencyPersonProjectAssignment", "Guid", true );
            
            CreateTable(
                "dbo._com_ccvonline_ResidencyCompetencyPersonProjectAssignmentAssessment",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ResidencyCompetencyPersonProjectAssignmentId = c.Int(nullable: false),
                        AssessmentDateTime = c.DateTime(),
                        Rating = c.Int(),
                        RatingNotes = c.String(),
                        Guid = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo._com_ccvonline_ResidencyCompetencyPersonProjectAssignment", t => t.ResidencyCompetencyPersonProjectAssignmentId)
                .Index(t => t.ResidencyCompetencyPersonProjectAssignmentId);

            // make sure to create index for Guid for new tables
            CreateIndex( "dbo._com_ccvonline_ResidencyCompetencyPersonProjectAssignmentAssessment", "Guid", true );
            
            CreateTable(
                "dbo._com_ccvonline_ResidencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessment",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ResidencyCompetencyPersonProjectAssignmentAssessmentId = c.Int(nullable: false),
                        ResidencyProjectPointOfAssessmentId = c.Int(nullable: false),
                        Rating = c.Int(),
                        RatingNotes = c.String(),
                        Guid = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo._com_ccvonline_ResidencyCompetencyPersonProjectAssignmentAssessment", t => t.ResidencyCompetencyPersonProjectAssignmentAssessmentId)
                .ForeignKey("dbo._com_ccvonline_ResidencyProjectPointOfAssessment", t => t.ResidencyProjectPointOfAssessmentId)
                .Index(t => t.ResidencyCompetencyPersonProjectAssignmentAssessmentId)
                .Index(t => t.ResidencyProjectPointOfAssessmentId);

            // make sure to create index for Guid for new tables
            CreateIndex( "dbo._com_ccvonline_ResidencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessment", "Guid", true );
            
            AlterColumn("dbo._com_ccvonline_ResidencyPeriod", "StartDate", c => c.DateTime(storeType: "date"));
            AlterColumn("dbo._com_ccvonline_ResidencyPeriod", "EndDate", c => c.DateTime(storeType: "date"));
            CreateIndex("dbo._com_ccvonline_ResidencyTrack", "ResidencyPeriodId");
            AddForeignKey("dbo._com_ccvonline_ResidencyTrack", "ResidencyPeriodId", "dbo._com_ccvonline_ResidencyPeriod", "Id");
        }

        /// <summary>
        /// Operations to be performed during the downgrade process.
        /// </summary>
        public override void Down()
        {
            DropForeignKey("dbo._com_ccvonline_ResidencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessment", "ResidencyProjectPointOfAssessmentId", "dbo._com_ccvonline_ResidencyProjectPointOfAssessment");
            DropForeignKey("dbo._com_ccvonline_ResidencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessment", "ResidencyCompetencyPersonProjectAssignmentAssessmentId", "dbo._com_ccvonline_ResidencyCompetencyPersonProjectAssignmentAssessment");
            DropForeignKey("dbo._com_ccvonline_ResidencyCompetencyPersonProjectAssignmentAssessment", "ResidencyCompetencyPersonProjectAssignmentId", "dbo._com_ccvonline_ResidencyCompetencyPersonProjectAssignment");
            DropForeignKey("dbo._com_ccvonline_ResidencyCompetencyPersonProjectAssignment", "AssessorPersonId", "dbo.Person");
            DropForeignKey("dbo._com_ccvonline_ResidencyCompetencyPersonProjectAssignment", "ResidencyCompetencyPersonProjectId", "dbo._com_ccvonline_ResidencyCompetencyPersonProject");
            DropForeignKey("dbo._com_ccvonline_ResidencyCompetencyPersonProject", "ResidencyCompetencyPersonId", "dbo.Person");
            DropForeignKey("dbo._com_ccvonline_ResidencyCompetencyPerson", "PersonId", "dbo.Person");
            DropForeignKey("dbo._com_ccvonline_ResidencyCompetencyPerson", "ResidencyCompetencyId", "dbo._com_ccvonline_ResidencyCompetency");
            DropForeignKey("dbo._com_ccvonline_ResidencyProjectPointOfAssessment", "ResidencyProjectId", "dbo._com_ccvonline_ResidencyProject");
            DropForeignKey("dbo._com_ccvonline_ResidencyProject", "ResidencyCompetencyId", "dbo._com_ccvonline_ResidencyCompetency");
            DropForeignKey("dbo._com_ccvonline_ResidencyCompetency", "FacilitatorPersonId", "dbo.Person");
            DropForeignKey("dbo._com_ccvonline_ResidencyCompetency", "TeacherOfRecordPersonId", "dbo.Person");
            DropForeignKey("dbo._com_ccvonline_ResidencyCompetency", "ResidencyTrackId", "dbo._com_ccvonline_ResidencyTrack");
            DropForeignKey("dbo._com_ccvonline_ResidencyTrack", "ResidencyPeriodId", "dbo._com_ccvonline_ResidencyPeriod");
            DropIndex("dbo._com_ccvonline_ResidencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessment", new[] { "ResidencyProjectPointOfAssessmentId" });
            DropIndex("dbo._com_ccvonline_ResidencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessment", new[] { "ResidencyCompetencyPersonProjectAssignmentAssessmentId" });
            DropIndex("dbo._com_ccvonline_ResidencyCompetencyPersonProjectAssignmentAssessment", new[] { "ResidencyCompetencyPersonProjectAssignmentId" });
            DropIndex("dbo._com_ccvonline_ResidencyCompetencyPersonProjectAssignment", new[] { "AssessorPersonId" });
            DropIndex("dbo._com_ccvonline_ResidencyCompetencyPersonProjectAssignment", new[] { "ResidencyCompetencyPersonProjectId" });
            DropIndex("dbo._com_ccvonline_ResidencyCompetencyPersonProject", new[] { "ResidencyCompetencyPersonId" });
            DropIndex("dbo._com_ccvonline_ResidencyCompetencyPerson", new[] { "PersonId" });
            DropIndex("dbo._com_ccvonline_ResidencyCompetencyPerson", new[] { "ResidencyCompetencyId" });
            DropIndex("dbo._com_ccvonline_ResidencyProjectPointOfAssessment", new[] { "ResidencyProjectId" });
            DropIndex("dbo._com_ccvonline_ResidencyProject", new[] { "ResidencyCompetencyId" });
            DropIndex("dbo._com_ccvonline_ResidencyCompetency", new[] { "FacilitatorPersonId" });
            DropIndex("dbo._com_ccvonline_ResidencyCompetency", new[] { "TeacherOfRecordPersonId" });
            DropIndex("dbo._com_ccvonline_ResidencyCompetency", new[] { "ResidencyTrackId" });
            DropIndex("dbo._com_ccvonline_ResidencyTrack", new[] { "ResidencyPeriodId" });
            AlterColumn("dbo._com_ccvonline_ResidencyPeriod", "EndDate", c => c.DateTimeOffset());
            AlterColumn("dbo._com_ccvonline_ResidencyPeriod", "StartDate", c => c.DateTimeOffset());
            DropTable("dbo._com_ccvonline_ResidencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessment");
            DropTable("dbo._com_ccvonline_ResidencyCompetencyPersonProjectAssignmentAssessment");
            DropTable("dbo._com_ccvonline_ResidencyCompetencyPersonProjectAssignment");
            DropTable("dbo._com_ccvonline_ResidencyCompetencyPersonProject");
            DropTable("dbo._com_ccvonline_ResidencyCompetencyPerson");
            DropTable("dbo._com_ccvonline_ResidencyProjectPointOfAssessment");
            DropTable("dbo._com_ccvonline_ResidencyProject");
            DropTable("dbo._com_ccvonline_ResidencyCompetency");
            CreateIndex("dbo._com_ccvonline_ResidencyTrack", "ResidencyPeriodId");
            AddForeignKey("dbo._com_ccvonline_ResidencyTrack", "ResidencyPeriodId", "dbo._com_ccvonline_ResidencyPeriod", "Id", cascadeDelete: true);
        }
    }
}
