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
    public partial class RenamePluginFolder : Rock.Migrations.RockMigration
    {
        /// <summary>
        /// Operations to be performed during the upgrade process.
        /// </summary>
        public override void Up()
        {
            Sql (@"
    UPDATE [BlockType] SET 
         [Path] = REPLACE( [Path], 'Plugins/com.ccvonline/Residency', 'Plugins/com_ccvonline/Residency' )
        ,[Name] = REPLACE( [Name], 'com .ccvonline - Residency', 'com_ccvonline - Residency' )
");
        }
        
        /// <summary>
        /// Operations to be performed during the downgrade process.
        /// </summary>
        public override void Down()
        {
            Sql( @"
    UPDATE [BlockType] SET 
         [Path] = REPLACE( [Path], 'Plugins/com_ccvonline/Residency', 'Plugins/com.ccvonline/Residency' )
        ,[Name] = REPLACE( [Name], 'com_ccvonline - Residency', 'com .ccvonline - Residency' )
" );
        }
    }
}
