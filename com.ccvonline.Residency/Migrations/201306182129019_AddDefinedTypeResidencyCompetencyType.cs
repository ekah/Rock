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
    public partial class AddDefinedTypeResidencyCompetencyType : Rock.Migrations.RockMigration
    {
        /// <summary>
        /// Operations to be performed during the upgrade process.
        /// </summary>
        public override void Up()
        {
            AddDefinedType( "Residency", "Residency Competency Type", "Used by the ccvonline Residency plugin to be assigned to a Residency Competency", com.ccvonline.Residency.SystemGuid.DefinedType.RESIDENCY_COMPETENCY_TYPE );

            AddDefinedValue( com.ccvonline.Residency.SystemGuid.DefinedType.RESIDENCY_COMPETENCY_TYPE, "Strategic Agility", "", "E9D1F7A6-4DD4-4D30-B629-72FE3FA58FEC", true );
            AddDefinedValue( com.ccvonline.Residency.SystemGuid.DefinedType.RESIDENCY_COMPETENCY_TYPE, "Operational Agility", "", "91214D34-8466-44F5-BB00-4736B1C36043", true );
            AddDefinedValue( com.ccvonline.Residency.SystemGuid.DefinedType.RESIDENCY_COMPETENCY_TYPE, "People Agility", "", "3929AD70-86C7-4F2E-9B91-A7FED4F7085C", true );
            AddDefinedValue( com.ccvonline.Residency.SystemGuid.DefinedType.RESIDENCY_COMPETENCY_TYPE, "Leadership Agility", "", "0DE0C7A1-E399-4447-8B9F-C5243DC2BEB4", true );
            AddDefinedValue( com.ccvonline.Residency.SystemGuid.DefinedType.RESIDENCY_COMPETENCY_TYPE, "Personal Composition", "", "C4DE3D73-7168-4AE1-AF7C-B849E7296D81", true );
        }
        
        /// <summary>
        /// Operations to be performed during the downgrade process.
        /// </summary>
        public override void Down()
        {
            DeleteDefinedValue( "E9D1F7A6-4DD4-4D30-B629-72FE3FA58FEC" );
            DeleteDefinedValue( "91214D34-8466-44F5-BB00-4736B1C36043" );
            DeleteDefinedValue( "3929AD70-86C7-4F2E-9B91-A7FED4F7085C" );
            DeleteDefinedValue( "0DE0C7A1-E399-4447-8B9F-C5243DC2BEB4" );
            DeleteDefinedValue( "C4DE3D73-7168-4AE1-AF7C-B849E7296D81" );

            DeleteDefinedType( com.ccvonline.Residency.SystemGuid.DefinedType.RESIDENCY_COMPETENCY_TYPE );
        }
    }
}
