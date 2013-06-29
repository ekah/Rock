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
    public partial class AssessmentOverallRatingColumn : Rock.Migrations.RockMigration
    {
        /// <summary>
        /// Operations to be performed during the upgrade process.
        /// </summary>
        public override void Up()
        {
            AddColumn("dbo._com_ccvonline_Residency_CompetencyPersonProjectAssignmentAssessment", "OverallRating", c => c.Decimal(precision: 2, scale: 1));
            DropColumn("dbo._com_ccvonline_Residency_CompetencyPersonProjectAssignmentAssessment", "Rating");
        }
        
        /// <summary>
        /// Operations to be performed during the downgrade process.
        /// </summary>
        public override void Down()
        {
            AddColumn("dbo._com_ccvonline_Residency_CompetencyPersonProjectAssignmentAssessment", "Rating", c => c.Int());
            DropColumn("dbo._com_ccvonline_Residency_CompetencyPersonProjectAssignmentAssessment", "OverallRating");
        }
    }
}
/* Skipped Operations for tables that are not part of ResidencyContext: Review these comments to verify the proper things were skipped */
/* To disable skipping, edit your Migrations\Configuration.cs so that CodeGenerator = new RockCSharpMigrationCodeGenerator<ResidencyContext>(false); */

// Up()...
// DropForeignKeyOperation for TableName PersonAccount, column PersonId.
// DropIndexOperation for TableName PersonAccount, column PersonId.
// DropTableOperation for TableName PersonAccount.

// Down()...
// CreateTableOperation for TableName PersonAccount.
// CreateIndexOperation for TableName PersonAccount, column PersonId.
// AddForeignKeyOperation for TableName PersonAccount, column PersonId.
