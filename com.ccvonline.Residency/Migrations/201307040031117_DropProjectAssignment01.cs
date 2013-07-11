//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//
namespace com.ccvonline.Residency.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    /// <summary>
    ///
    /// </summary>
    public partial class DropProjectAssignment01 : Rock.Migrations.RockMigration
    {
        /// <summary>
        /// Operations to be performed during the upgrade process.
        /// </summary>
        public override void Up()
        {
            // delete ProjectAssignment related blocktypes and pages
            Sql( @"
delete from [BlockType] where [Guid] in (
'0B86F65A-904B-4FEE-86BF-99E1C1A696F5',
'203C01AB-0EE6-4EE0-A934-EB0FAF426E9C',
'8A5FB3E3-4147-4DE0-9CAE-20974ADD5E70',
'9C373F55-7E44-4641-AA6B-A30E7F214F37',
'9EEA2EF7-E49D-4CAB-B067-BE4ECB9FF376',
'A102FA19-85A6-4701-9C14-29F1A412FDC0'
)
" );

            Sql( @"delete from [Page] where [Guid] in (
'165E7CB7-E15A-4AC2-8383-567A593279F0',
'32E7BCDE-37BC-48B5-B0FE-AA784AF0425A',
'69A714EB-1870-4516-AB4F-63ADF2100FEA',
'B8DDC9C7-4A30-4AEB-B37A-6B4A62DD2450',
'DFFFABE1-B6C6-450A-9611-14A74386CC47'
)" );
            
            DropForeignKey("dbo._com_ccvonline_Residency_Competency", "CompetencyTypeValueId", "dbo.DefinedValue");
            DropForeignKey("dbo._com_ccvonline_Residency_CompetencyPersonProjectAssignment", "CompetencyPersonProjectId", "dbo._com_ccvonline_Residency_CompetencyPersonProject");
            DropForeignKey("dbo._com_ccvonline_Residency_CompetencyPersonProjectAssignment", "AssessorPersonId", "dbo.Person");
            DropForeignKey("dbo._com_ccvonline_Residency_CompetencyPersonProjectAssignmentAssessment", "CompetencyPersonProjectAssignmentId", "dbo._com_ccvonline_Residency_CompetencyPersonProjectAssignment");
            DropForeignKey("dbo._com_ccvonline_Residency_CompetencyPersonProjectAssignmentAssessmentPointOfAssessment", "CompetencyPersonProjectAssignmentAssessmentId", "dbo._com_ccvonline_Residency_CompetencyPersonProjectAssignmentAssessment");
            DropForeignKey("dbo._com_ccvonline_Residency_CompetencyPersonProjectAssignmentAssessmentPointOfAssessment", "ProjectPointOfAssessmentId", "dbo._com_ccvonline_Residency_ProjectPointOfAssessment");
            DropIndex("dbo._com_ccvonline_Residency_Competency", new[] { "CompetencyTypeValueId" });
            DropIndex("dbo._com_ccvonline_Residency_CompetencyPersonProjectAssignment", new[] { "CompetencyPersonProjectId" });
            DropIndex("dbo._com_ccvonline_Residency_CompetencyPersonProjectAssignment", new[] { "AssessorPersonId" });
            DropIndex("dbo._com_ccvonline_Residency_CompetencyPersonProjectAssignmentAssessment", new[] { "CompetencyPersonProjectAssignmentId" });
            DropIndex("dbo._com_ccvonline_Residency_CompetencyPersonProjectAssignmentAssessmentPointOfAssessment", new[] { "CompetencyPersonProjectAssignmentAssessmentId" });
            DropIndex("dbo._com_ccvonline_Residency_CompetencyPersonProjectAssignmentAssessmentPointOfAssessment", new[] { "ProjectPointOfAssessmentId" });
            CreateTable(
                "dbo._com_ccvonline_Residency_CompetencyPersonProjectAssessment",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CompetencyPersonProjectId = c.Int(nullable: false),
                        AssessorPersonId = c.Int(),
                        AssessmentDateTime = c.DateTime(),
                        OverallRating = c.Decimal(precision: 2, scale: 1),
                        RatingNotes = c.String(),
                        ResidentComments = c.String(),
                        Guid = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo._com_ccvonline_Residency_CompetencyPersonProject", t => t.CompetencyPersonProjectId)
                .ForeignKey("dbo.Person", t => t.AssessorPersonId)
                .Index(t => t.CompetencyPersonProjectId)
                .Index(t => t.AssessorPersonId);
            
            CreateIndex( "dbo._com_ccvonline_Residency_CompetencyPersonProjectAssessment", "Guid", true );
            CreateTable(
                "dbo._com_ccvonline_Residency_CompetencyPersonProjectAssessmentPointOfAssessment",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CompetencyPersonProjectAssessmentId = c.Int(nullable: false),
                        ProjectPointOfAssessmentId = c.Int(nullable: false),
                        Rating = c.Int(),
                        RatingNotes = c.String(),
                        Guid = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo._com_ccvonline_Residency_CompetencyPersonProjectAssessment", t => t.CompetencyPersonProjectAssessmentId)
                .ForeignKey("dbo._com_ccvonline_Residency_ProjectPointOfAssessment", t => t.ProjectPointOfAssessmentId)
                .Index(t => t.CompetencyPersonProjectAssessmentId)
                .Index(t => t.ProjectPointOfAssessmentId);
            
            CreateIndex( "dbo._com_ccvonline_Residency_CompetencyPersonProjectAssessmentPointOfAssessment", "Guid", true );
            AddColumn("dbo._com_ccvonline_Residency_Project", "MinAssessmentCountDefault", c => c.Int());
            AddColumn("dbo._com_ccvonline_Residency_ProjectPointOfAssessment", "CompetencyTypeValueId", c => c.Int());
            AddColumn("dbo._com_ccvonline_Residency_CompetencyPersonProject", "MinAssessmentCount", c => c.Int());
            CreateIndex("dbo._com_ccvonline_Residency_ProjectPointOfAssessment", "CompetencyTypeValueId");
            AddForeignKey("dbo._com_ccvonline_Residency_ProjectPointOfAssessment", "CompetencyTypeValueId", "dbo.DefinedValue", "Id");
            DropColumn("dbo._com_ccvonline_Residency_Competency", "CompetencyTypeValueId");
            DropColumn("dbo._com_ccvonline_Residency_Project", "MinAssignmentCountDefault");
            DropColumn("dbo._com_ccvonline_Residency_CompetencyPersonProject", "MinAssignmentCount");
            DropTable("dbo._com_ccvonline_Residency_CompetencyPersonProjectAssignment");
            DropTable("dbo._com_ccvonline_Residency_CompetencyPersonProjectAssignmentAssessment");
            DropTable("dbo._com_ccvonline_Residency_CompetencyPersonProjectAssignmentAssessmentPointOfAssessment");
        }
        
        /// <summary>
        /// Operations to be performed during the downgrade process.
        /// </summary>
        public override void Down()
        {
            CreateTable(
                "dbo._com_ccvonline_Residency_CompetencyPersonProjectAssignmentAssessmentPointOfAssessment",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CompetencyPersonProjectAssignmentAssessmentId = c.Int(nullable: false),
                        ProjectPointOfAssessmentId = c.Int(nullable: false),
                        Rating = c.Int(),
                        RatingNotes = c.String(),
                        Guid = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo._com_ccvonline_Residency_CompetencyPersonProjectAssignmentAssessment",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CompetencyPersonProjectAssignmentId = c.Int(nullable: false),
                        AssessmentDateTime = c.DateTime(),
                        OverallRating = c.Decimal(precision: 2, scale: 1),
                        RatingNotes = c.String(),
                        ResidentComments = c.String(),
                        Guid = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo._com_ccvonline_Residency_CompetencyPersonProjectAssignment",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CompetencyPersonProjectId = c.Int(nullable: false),
                        AssessorPersonId = c.Int(),
                        CompletedDateTime = c.DateTime(),
                        Guid = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo._com_ccvonline_Residency_CompetencyPersonProject", "MinAssignmentCount", c => c.Int());
            AddColumn("dbo._com_ccvonline_Residency_Project", "MinAssignmentCountDefault", c => c.Int());
            AddColumn("dbo._com_ccvonline_Residency_Competency", "CompetencyTypeValueId", c => c.Int());
            DropForeignKey("dbo._com_ccvonline_Residency_CompetencyPersonProjectAssessmentPointOfAssessment", "ProjectPointOfAssessmentId", "dbo._com_ccvonline_Residency_ProjectPointOfAssessment");
            DropForeignKey("dbo._com_ccvonline_Residency_CompetencyPersonProjectAssessmentPointOfAssessment", "CompetencyPersonProjectAssessmentId", "dbo._com_ccvonline_Residency_CompetencyPersonProjectAssessment");
            DropForeignKey("dbo._com_ccvonline_Residency_CompetencyPersonProjectAssessment", "AssessorPersonId", "dbo.Person");
            DropForeignKey("dbo._com_ccvonline_Residency_CompetencyPersonProjectAssessment", "CompetencyPersonProjectId", "dbo._com_ccvonline_Residency_CompetencyPersonProject");
            DropForeignKey("dbo._com_ccvonline_Residency_ProjectPointOfAssessment", "CompetencyTypeValueId", "dbo.DefinedValue");
            DropIndex("dbo._com_ccvonline_Residency_CompetencyPersonProjectAssessmentPointOfAssessment", new[] { "ProjectPointOfAssessmentId" });
            DropIndex("dbo._com_ccvonline_Residency_CompetencyPersonProjectAssessmentPointOfAssessment", new[] { "CompetencyPersonProjectAssessmentId" });
            DropIndex("dbo._com_ccvonline_Residency_CompetencyPersonProjectAssessment", new[] { "AssessorPersonId" });
            DropIndex("dbo._com_ccvonline_Residency_CompetencyPersonProjectAssessment", new[] { "CompetencyPersonProjectId" });
            DropIndex("dbo._com_ccvonline_Residency_ProjectPointOfAssessment", new[] { "CompetencyTypeValueId" });
            DropColumn("dbo._com_ccvonline_Residency_CompetencyPersonProject", "MinAssessmentCount");
            DropColumn("dbo._com_ccvonline_Residency_ProjectPointOfAssessment", "CompetencyTypeValueId");
            DropColumn("dbo._com_ccvonline_Residency_Project", "MinAssessmentCountDefault");
            DropTable("dbo._com_ccvonline_Residency_CompetencyPersonProjectAssessmentPointOfAssessment");
            DropTable("dbo._com_ccvonline_Residency_CompetencyPersonProjectAssessment");
            CreateIndex("dbo._com_ccvonline_Residency_CompetencyPersonProjectAssignmentAssessmentPointOfAssessment", "ProjectPointOfAssessmentId");
            CreateIndex("dbo._com_ccvonline_Residency_CompetencyPersonProjectAssignmentAssessmentPointOfAssessment", "CompetencyPersonProjectAssignmentAssessmentId");
            CreateIndex("dbo._com_ccvonline_Residency_CompetencyPersonProjectAssignmentAssessment", "CompetencyPersonProjectAssignmentId");
            CreateIndex("dbo._com_ccvonline_Residency_CompetencyPersonProjectAssignment", "AssessorPersonId");
            CreateIndex("dbo._com_ccvonline_Residency_CompetencyPersonProjectAssignment", "CompetencyPersonProjectId");
            CreateIndex("dbo._com_ccvonline_Residency_Competency", "CompetencyTypeValueId");
            AddForeignKey("dbo._com_ccvonline_Residency_CompetencyPersonProjectAssignmentAssessmentPointOfAssessment", "ProjectPointOfAssessmentId", "dbo._com_ccvonline_Residency_ProjectPointOfAssessment", "Id");
            AddForeignKey("dbo._com_ccvonline_Residency_CompetencyPersonProjectAssignmentAssessmentPointOfAssessment", "CompetencyPersonProjectAssignmentAssessmentId", "dbo._com_ccvonline_Residency_CompetencyPersonProjectAssignmentAssessment", "Id");
            AddForeignKey("dbo._com_ccvonline_Residency_CompetencyPersonProjectAssignmentAssessment", "CompetencyPersonProjectAssignmentId", "dbo._com_ccvonline_Residency_CompetencyPersonProjectAssignment", "Id");
            AddForeignKey("dbo._com_ccvonline_Residency_CompetencyPersonProjectAssignment", "AssessorPersonId", "dbo.Person", "Id");
            AddForeignKey("dbo._com_ccvonline_Residency_CompetencyPersonProjectAssignment", "CompetencyPersonProjectId", "dbo._com_ccvonline_Residency_CompetencyPersonProject", "Id");
            AddForeignKey("dbo._com_ccvonline_Residency_Competency", "CompetencyTypeValueId", "dbo.DefinedValue", "Id");
        }
    }
}
