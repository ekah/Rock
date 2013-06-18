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
            CreateIndex( "_com_ccvonline_Residency_CompetencyPerson", new string[] { "CompetencyId", "PersonId" }, true );
            CreateIndex( "_com_ccvonline_Residency_CompetencyPersonProject", new string[] { "CompetencyPersonId", "ProjectId" }, true );
            CreateIndex( "_com_ccvonline_Residency_ProjectPointOfAssessment", new string[] { "ProjectId", "AssessmentOrder" }, true );
        }

        /// <summary>
        /// Operations to be performed during the downgrade process.
        /// </summary>
        public override void Down()
        {
            DropIndex( "_com_ccvonline_Residency_CompetencyPerson", new string[] { "CompetencyId", "PersonId" } );
            DropIndex( "_com_ccvonline_Residency_CompetencyPersonProject", new string[] { "CompetencyPersonId", "ProjectId" } );
            DropIndex( "_com_ccvonline_Residency_ProjectPointOfAssessment", new string[] { "ProjectId", "AssessmentOrder" } );
        }
    }
}
