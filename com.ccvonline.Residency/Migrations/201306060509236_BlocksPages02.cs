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
    public partial class BlocksPages02 : Rock.Migrations.RockMigration
    {
        /// <summary>
        /// Operations to be performed during the upgrade process.
        /// </summary>
        public override void Up()
        {
            AddBlock( "82B81403-8A93-4F42-A958-5303C3AF1508", "F49AD5F8-1E45-41E7-A88E-8CD285815BD9", "Page Xslt Transformation", "", "Content", 0, "149C35E9-25AD-4B99-BFB7-5CCF4A6E6ACA" );

            // Attrib for BlockType: com .ccvonline - Residency Competency Detail:Residency Track Page
            AddBlockTypeAttribute( "D1D1C418-B84B-4307-B4EC-D2FD2970D639", "BD53F9C9-EBA9-4D3F-82EA-DE5DD34A8108", "Residency Track Page", "ResidencyTrackPage", "", "", 0, "", "32DB890B-C394-4CA0-ACD2-3D2EA8E9C8F5" );

            // Attrib Value for Page Xslt Transformation:Root Page
            AddBlockAttributeValue( "149C35E9-25AD-4B99-BFB7-5CCF4A6E6ACA", "DD516FA7-966E-4C80-8523-BEAC91C8EEDA", "fd705eed-cd8d-4f53-8c16-abba15cc27d5" );

            // Attrib Value for Page Xslt Transformation:XSLT File
            AddBlockAttributeValue( "149C35E9-25AD-4B99-BFB7-5CCF4A6E6ACA", "D8A029F8-83BE-454A-99D3-94D879EBF87C", "~/Assets/XSLT/PageListAsBlocks.xslt" );

            // Attrib Value for Page Xslt Transformation:Number of Levels
            AddBlockAttributeValue( "149C35E9-25AD-4B99-BFB7-5CCF4A6E6ACA", "9909E07F-0E68-43B8-A151-24D03C795093", "1" );

            // Attrib Value for Page Xslt Transformation:Include Current Parameters
            AddBlockAttributeValue( "149C35E9-25AD-4B99-BFB7-5CCF4A6E6ACA", "A0B1F15A-8735-48CA-AFF5-10ED7DD24EA7", "False" );

            // Attrib Value for Residency Competency Detail:Residency Track Page
            AddBlockAttributeValue( "59C12E5C-7478-4E24-843C-14561C47FBD1", "32DB890B-C394-4CA0-ACD2-3D2EA8E9C8F5", "038aef17-65ee-4161-bf9e-64aacc791701" );

            // hide breadcrumb name for most of the residency admin pages
            Sql( @"
update [Page] set [BreadCrumbDisplayName] = 0 where [Guid] in 
  ('FD705EED-CD8D-4F53-8C16-ABBA15CC27D5', 'F8D8663B-FE4F-4F48-A359-DBE656AE69A2', '038AEF17-65EE-4161-BF9E-64AACC791701','2BD2E7BB-4199-4C18-B51A-AA3755DECD1B','37BA8EAD-16C5-4257-953D-D202684A8E61','DD65505A-6FE2-4478-8901-9F38F484E3EB')"
                );

        }
        
        /// <summary>
        /// Operations to be performed during the downgrade process.
        /// </summary>
        public override void Down()
        {
            Sql( @"
update [Page] set [BreadCrumbDisplayName] = 1 where [Guid] in 
  ('FD705EED-CD8D-4F53-8C16-ABBA15CC27D5', 'F8D8663B-FE4F-4F48-A359-DBE656AE69A2', '038AEF17-65EE-4161-BF9E-64AACC791701','2BD2E7BB-4199-4C18-B51A-AA3755DECD1B','37BA8EAD-16C5-4257-953D-D202684A8E61','DD65505A-6FE2-4478-8901-9F38F484E3EB')"
                 );
            
            DeleteAttribute( "32DB890B-C394-4CA0-ACD2-3D2EA8E9C8F5" ); // Residency Track Page
            DeleteBlock( "149C35E9-25AD-4B99-BFB7-5CCF4A6E6ACA" ); // Page Xslt Transformation
        }
    }
}
