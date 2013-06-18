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
    public partial class RenameTables01 : Rock.Migrations.RockMigration
    {
        /// <summary>
        /// Operations to be performed during the upgrade process.
        /// </summary>
        public override void Up()
        {
            RenameTable(name: "dbo._com_ccvonline_ResidencyCompetency", newName: "_com_ccvonline_Residency_Competency");
            RenameTable(name: "dbo._com_ccvonline_ResidencyTrack", newName: "_com_ccvonline_Residency_Track");
            RenameTable(name: "dbo._com_ccvonline_ResidencyPeriod", newName: "_com_ccvonline_Residency_Period");
            RenameTable(name: "dbo._com_ccvonline_ResidencyProject", newName: "_com_ccvonline_Residency_Project");
            RenameTable(name: "dbo._com_ccvonline_ResidencyProjectPointOfAssessment", newName: "_com_ccvonline_Residency_ProjectPointOfAssessment");
            RenameTable(name: "dbo._com_ccvonline_ResidencyCompetencyPerson", newName: "_com_ccvonline_Residency_CompetencyPerson");
            RenameTable(name: "dbo._com_ccvonline_ResidencyCompetencyPersonProject", newName: "_com_ccvonline_Residency_CompetencyPersonProject");
            RenameTable(name: "dbo._com_ccvonline_ResidencyCompetencyPersonProjectAssignment", newName: "_com_ccvonline_Residency_CompetencyPersonProjectAssignment");
            RenameTable(name: "dbo._com_ccvonline_ResidencyCompetencyPersonProjectAssignmentAssessment", newName: "_com_ccvonline_Residency_CompetencyPersonProjectAssignmentAssessment");
            RenameTable(name: "dbo._com_ccvonline_ResidencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessment", newName: "_com_ccvonline_Residency_CompetencyPersonProjectAssignmentAssessmentPointOfAssessment");
        }
        
        /// <summary>
        /// Operations to be performed during the downgrade process.
        /// </summary>
        public override void Down()
        {
            RenameTable(name: "dbo._com_ccvonline_Residency_CompetencyPersonProjectAssignmentAssessmentPointOfAssessment", newName: "_com_ccvonline_ResidencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessment");
            RenameTable(name: "dbo._com_ccvonline_Residency_CompetencyPersonProjectAssignmentAssessment", newName: "_com_ccvonline_ResidencyCompetencyPersonProjectAssignmentAssessment");
            RenameTable(name: "dbo._com_ccvonline_Residency_CompetencyPersonProjectAssignment", newName: "_com_ccvonline_ResidencyCompetencyPersonProjectAssignment");
            RenameTable(name: "dbo._com_ccvonline_Residency_CompetencyPersonProject", newName: "_com_ccvonline_ResidencyCompetencyPersonProject");
            RenameTable(name: "dbo._com_ccvonline_Residency_CompetencyPerson", newName: "_com_ccvonline_ResidencyCompetencyPerson");
            RenameTable(name: "dbo._com_ccvonline_Residency_ProjectPointOfAssessment", newName: "_com_ccvonline_ResidencyProjectPointOfAssessment");
            RenameTable(name: "dbo._com_ccvonline_Residency_Project", newName: "_com_ccvonline_ResidencyProject");
            RenameTable(name: "dbo._com_ccvonline_Residency_Period", newName: "_com_ccvonline_ResidencyPeriod");
            RenameTable(name: "dbo._com_ccvonline_Residency_Track", newName: "_com_ccvonline_ResidencyTrack");
            RenameTable(name: "dbo._com_ccvonline_Residency_Competency", newName: "_com_ccvonline_ResidencyCompetency");
        }
    }
}
