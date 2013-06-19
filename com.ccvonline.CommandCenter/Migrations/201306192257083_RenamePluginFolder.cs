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
    public partial class RenamePluginFolder : Rock.Migrations.RockMigration
    {
        /// <summary>
        /// Operations to be performed during the upgrade process.
        /// </summary>
        public override void Up()
        {
            Sql( @"
    UPDATE [BlockType] SET 
         [Path] = REPLACE( [Path], 'Plugins/com.ccvonline/CommandCenter', 'Plugins/com_ccvonline/CommandCenter' )
        ,[Name] = REPLACE( [Name], 'com .ccvonline -  Command Center', 'com_ccvonline - Command Center' )
" );
        }

        /// <summary>
        /// Operations to be performed during the downgrade process.
        /// </summary>
        public override void Down()
        {
            Sql( @"
    UPDATE [BlockType] SET 
         [Path] = REPLACE( [Path], 'Plugins/com_ccvonline/CommandCenter', 'Plugins/com.ccvonline/CommandCenter' )
        ,[Name] = REPLACE( [Name], 'com_ccvonline -  Command Center', 'com .ccvonline -  Command Center' )
" );
        }
    }
}
