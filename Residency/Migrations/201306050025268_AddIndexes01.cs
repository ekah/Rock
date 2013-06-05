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
    public partial class AddIndexes01 : Rock.Migrations.RockMigration
    {
        /// <summary>
        /// Operations to be performed during the upgrade process.
        /// </summary>
        public override void Up()
        {
            CreateIndex( "_com_ccvonline_ResidencyCompetency", new string[] { "ResidencyTrackId", "Name" }, true );
            CreateIndex( "_com_ccvonline_ResidencyCompetencyPerson", new string[] { "ResidencyCompetencyId", "PersonId" }, true );
            CreateIndex( "_com_ccvonline_ResidencyCompetencyPersonProject", new string[] { "ResidencyCompetencyPersonId", "ResidencyProjectId" }, true );
            CreateIndex( "_com_ccvonline_ResidencyProject", new string[] { "ResidencyCompetencyId", "Name" }, true );
            CreateIndex( "_com_ccvonline_ResidencyProjectPointOfAssessment", new string[] { "ResidencyProjectId", "AssessmentOrder" }, true );
            CreateIndex( "_com_ccvonline_ResidencyTrack", new string[] { "ResidencyPeriodId", "Name" }, true );
        }
        
        /// <summary>
        /// Operations to be performed during the downgrade process.
        /// </summary>
        public override void Down()
        {
            DropIndex( "_com_ccvonline_ResidencyCompetency", new string[] { "ResidencyTrackId", "Name" } );
            DropIndex( "_com_ccvonline_ResidencyCompetencyPerson", new string[] { "ResidencyCompetencyId", "PersonId" } );
            DropIndex( "_com_ccvonline_ResidencyCompetencyPersonProject", new string[] { "ResidencyCompetencyPersonId", "ResidencyProjectId" } );
            DropIndex( "_com_ccvonline_ResidencyProject", new string[] { "ResidencyCompetencyId", "Name" } );
            DropIndex( "_com_ccvonline_ResidencyProjectPointOfAssessment", new string[] { "ResidencyProjectId", "AssessmentOrder" } );
            DropIndex( "_com_ccvonline_ResidencyTrack", new string[] { "ResidencyPeriodId", "Name" } );
        }
    }
}
