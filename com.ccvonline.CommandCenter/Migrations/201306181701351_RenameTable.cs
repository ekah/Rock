//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//
namespace com.ccvonline.CommandCenter.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    /// <summary>
    ///
    /// </summary>
    public partial class RenameTable : Rock.Migrations.RockMigration
    {
        /// <summary>
        /// Operations to be performed during the upgrade process.
        /// </summary>
        public override void Up()
        {
            RenameTable(name: "dbo._com_ccvonline_CommandCenterRecording", newName: "_com_ccvonline_CommandCenter_Recording");
        }
        
        /// <summary>
        /// Operations to be performed during the downgrade process.
        /// </summary>
        public override void Down()
        {
            RenameTable(name: "dbo._com_ccvonline_CommandCenter_Recording", newName: "_com_ccvonline_CommandCenterRecording");
        }
    }
}
/* Skipped Operations for tables that are not part of CommandCenterContext: Review these comments to verify the proper things were skipped */
/* To disable skipping, edit your Migrations\Configuration.cs so that CodeGenerator = new RockCSharpMigrationCodeGenerator<CommandCenterContext>(false); */

// Up()...

// Down()...
// RenameTableOperation for TableName _com_ccvonline_CommandCenterRecording.
