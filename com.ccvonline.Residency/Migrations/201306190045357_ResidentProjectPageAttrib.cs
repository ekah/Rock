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
    public partial class ResidentProjectPageAttrib : Rock.Migrations.RockMigration
    {
        /// <summary>
        /// Operations to be performed during the upgrade process.
        /// </summary>
        public override void Up()
        {
            // Attrib for BlockType: com .ccvonline - Residency Competency Person Project Assignment Detail:Resident Project Page
            AddBlockTypeAttribute( "8A5FB3E3-4147-4DE0-9CAE-20974ADD5E70", "BD53F9C9-EBA9-4D3F-82EA-DE5DD34A8108", "Resident Project Page", "ResidentProjectPage", "", "", 0, "", "499D97C8-EABC-4D98-B3F0-BDD2377B434C" );
        }
        
        /// <summary>
        /// Operations to be performed during the downgrade process.
        /// </summary>
        public override void Down()
        {
            // Attrib for BlockType: com .ccvonline - Residency Competency Person Project Assignment Detail:Resident Project Page
            DeleteAttribute( "499D97C8-EABC-4D98-B3F0-BDD2377B434C" );
        }
    }
}
