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
    public partial class ResidentProjectAssessmentPages : Rock.Migrations.RockMigration
    {
        /// <summary>
        /// Operations to be performed during the upgrade process.
        /// </summary>
        public override void Up()
        {
            AddPage( "F98B0061-8327-4B96-8A5E-B3C58D899B31", "Assessments", "", "Default", "162927F6-E503-43C4-B075-55F1E592E96E", "" );
            AddPage( "162927F6-E503-43C4-B075-55F1E592E96E", "Project Assessment Detail", "", "Default", "BDA4C473-01CD-449E-97D4-4B054E3F0959", "" );

            AddBlock( "826C0BFF-C831-4427-98F9-57FF462D82F5", "F7193487-1234-49D7-9CEC-7F5F452B7E3F", "Current Person", "", "Content", 0, "E517DDD7-73DB-4475-87A4-83CBCD7657F1" );
            AddBlock( "826C0BFF-C831-4427-98F9-57FF462D82F5", "F49AD5F8-1E45-41E7-A88E-8CD285815BD9", "Page Xslt Transformation", "", "Content", 1, "D07780FC-0ED5-4881-8B76-24F6FAE8A897" );
            AddBlock( "162927F6-E503-43C4-B075-55F1E592E96E", "F7193487-1234-49D7-9CEC-7F5F452B7E3F", "Current Person", "", "Content", 0, "4EFF1322-6A9A-44A0-B3B8-CB547CB09C0B" );
            AddBlock( "162927F6-E503-43C4-B075-55F1E592E96E", "F49AD5F8-1E45-41E7-A88E-8CD285815BD9", "Page Xslt Transformation", "", "Content", 1, "3938E111-C9FF-49E9-B1B8-A2AA89080F51" );
            AddBlock( "162927F6-E503-43C4-B075-55F1E592E96E", "BF27EBA9-EA1D-4B15-8B43-D97F0C1B0B20", "Resident Project Assessment List", "", "Content", 2, "B459F23A-9C32-4537-BA93-637A81ACB35A" );
            AddBlock( "BDA4C473-01CD-449E-97D4-4B054E3F0959", "D2835421-1D69-4D2E-80BC-836FF606ADDD", "Resident Project Assessment Detail", "", "Content", 0, "1DB06C26-B318-46CD-9E9F-219FC1EF6338" );

            // Attrib for BlockType: com_ccvonline - Residency - Resident Project Assessment List:Show Competency Column
            AddBlockTypeAttribute( "BF27EBA9-EA1D-4B15-8B43-D97F0C1B0B20", "1EDAFDED-DFE6-4334-B019-6EECBA89E05A", "Show Competency Column", "ShowCompetencyColumn", "", "", 0, "False", "BFE19B2F-D62D-4C87-B928-FFFAE65BD12E" );

            // Attrib for BlockType: com_ccvonline - Residency - Resident Project Assessment List:Show Project Column
            AddBlockTypeAttribute( "BF27EBA9-EA1D-4B15-8B43-D97F0C1B0B20", "1EDAFDED-DFE6-4334-B019-6EECBA89E05A", "Show Project Column", "ShowProjectColumn", "", "", 0, "False", "F0390AAB-D114-4367-8349-35EEA8EDACB8" );

            // Attrib for BlockType: com_ccvonline - Residency - Resident Project Assessment List:Show Grid Title
            AddBlockTypeAttribute( "BF27EBA9-EA1D-4B15-8B43-D97F0C1B0B20", "1EDAFDED-DFE6-4334-B019-6EECBA89E05A", "Show Grid Title", "ShowGridTitle", "", "", 0, "False", "F5DAD59C-AC05-4BCA-ACC3-171924039872" );


            // Attrib Value for Block:Resident Project Assessment List, Attribute:Show Competency Column, Page:Resident Project Detail
            AddBlockAttributeValue( "CA0F81CA-17B1-44EA-B8DF-C834DF8ED43F", "BFE19B2F-D62D-4C87-B928-FFFAE65BD12E", "False" );

            // Attrib Value for Block:Resident Project Assessment List, Attribute:Show Project Column, Page:Resident Project Detail
            AddBlockAttributeValue( "CA0F81CA-17B1-44EA-B8DF-C834DF8ED43F", "F0390AAB-D114-4367-8349-35EEA8EDACB8", "False" );

            // Attrib Value for Block:Resident Project Assessment List, Attribute:Show Grid Title, Page:Resident Project Detail
            AddBlockAttributeValue( "CA0F81CA-17B1-44EA-B8DF-C834DF8ED43F", "F5DAD59C-AC05-4BCA-ACC3-171924039872", "True" );

            // Attrib Value for Block:Page Xslt Transformation, Attribute:Include Current Parameters, Page:Coursework
            AddBlockAttributeValue( "D07780FC-0ED5-4881-8B76-24F6FAE8A897", "A0B1F15A-8735-48CA-AFF5-10ED7DD24EA7", "True" );

            // Attrib Value for Block:Page Xslt Transformation, Attribute:Include Current QueryString, Page:Coursework
            AddBlockAttributeValue( "D07780FC-0ED5-4881-8B76-24F6FAE8A897", "09D9AB6B-B2E6-4E9C-AAFE-25758BE2754B", "True" );

            // Attrib Value for Block:Page Xslt Transformation, Attribute:Root Page, Page:Coursework
            AddBlockAttributeValue( "D07780FC-0ED5-4881-8B76-24F6FAE8A897", "DD516FA7-966E-4C80-8523-BEAC91C8EEDA", "f98b0061-8327-4b96-8a5e-b3c58d899b31" );

            // Attrib Value for Block:Page Xslt Transformation, Attribute:XSLT File, Page:Coursework
            AddBlockAttributeValue( "D07780FC-0ED5-4881-8B76-24F6FAE8A897", "D8A029F8-83BE-454A-99D3-94D879EBF87C", "~/Themes/RockChMS/Assets/Xslt/SubPageNav.xslt" );

            // Attrib Value for Block:Page Xslt Transformation, Attribute:Number of Levels, Page:Coursework
            AddBlockAttributeValue( "D07780FC-0ED5-4881-8B76-24F6FAE8A897", "9909E07F-0E68-43B8-A151-24D03C795093", "3" );

            // Attrib Value for Block:Page Xslt Transformation, Attribute:Include Current Parameters, Page:Assessments
            AddBlockAttributeValue( "3938E111-C9FF-49E9-B1B8-A2AA89080F51", "A0B1F15A-8735-48CA-AFF5-10ED7DD24EA7", "True" );

            // Attrib Value for Block:Page Xslt Transformation, Attribute:Include Current QueryString, Page:Assessments
            AddBlockAttributeValue( "3938E111-C9FF-49E9-B1B8-A2AA89080F51", "09D9AB6B-B2E6-4E9C-AAFE-25758BE2754B", "True" );

            // Attrib Value for Block:Page Xslt Transformation, Attribute:Root Page, Page:Assessments
            AddBlockAttributeValue( "3938E111-C9FF-49E9-B1B8-A2AA89080F51", "DD516FA7-966E-4C80-8523-BEAC91C8EEDA", "f98b0061-8327-4b96-8a5e-b3c58d899b31" );

            // Attrib Value for Block:Page Xslt Transformation, Attribute:XSLT File, Page:Assessments
            AddBlockAttributeValue( "3938E111-C9FF-49E9-B1B8-A2AA89080F51", "D8A029F8-83BE-454A-99D3-94D879EBF87C", "~/Themes/RockChMS/Assets/Xslt/SubPageNav.xslt" );

            // Attrib Value for Block:Page Xslt Transformation, Attribute:Number of Levels, Page:Assessments
            AddBlockAttributeValue( "3938E111-C9FF-49E9-B1B8-A2AA89080F51", "9909E07F-0E68-43B8-A151-24D03C795093", "3" );

            // Attrib Value for Block:Resident Project Assessment List, Attribute:Show Grid Title, Page:Assessments
            AddBlockAttributeValue( "B459F23A-9C32-4537-BA93-637A81ACB35A", "F5DAD59C-AC05-4BCA-ACC3-171924039872", "False" );

            // Attrib Value for Block:Resident Project Assessment List, Attribute:Show Competency Column, Page:Assessments
            AddBlockAttributeValue( "B459F23A-9C32-4537-BA93-637A81ACB35A", "BFE19B2F-D62D-4C87-B928-FFFAE65BD12E", "True" );

            // Attrib Value for Block:Resident Project Assessment List, Attribute:Show Project Column, Page:Assessments
            AddBlockAttributeValue( "B459F23A-9C32-4537-BA93-637A81ACB35A", "F0390AAB-D114-4367-8349-35EEA8EDACB8", "True" );

            // Attrib Value for Block:Resident Project Assessment List, Attribute:Detail Page, Page:Assessments
            AddBlockAttributeValue( "B459F23A-9C32-4537-BA93-637A81ACB35A", "B6ABAF24-2D2E-4A0E-A195-C66E98110D2D", "bda4c473-01cd-449e-97d4-4b054e3f0959" );


            // Update new pages to use Residency Site
            Sql( @"
DECLARE @PageId int
DECLARE @SiteId int
DECLARE @LoginPageReference NVARCHAR(260)

-- 'Resident Home' page is default page for site
SET @PageId = (SELECT [Id] FROM [Page] WHERE [Guid] = '826C0BFF-C831-4427-98F9-57FF462D82F5')

SET @LoginPageReference = (select '~/page/' + CONVERT(nvarchar(10), [Id]) from [Page] where [Guid] = '07770489-9C8D-43FA-85B3-E99BB54D3353')

SET @SiteId = (select [Id] from [Site] where [Guid] = '960F1D98-891A-4508-8F31-3CF206F5406E')

-- Update Resident pages to use new site (default layout)
UPDATE [Page] SET 
    [SiteId] = @SiteId,
    [Layout] = 'Default'
WHERE [Guid] in (
'162927F6-E503-43C4-B075-55F1E592E96E',
'BDA4C473-01CD-449E-97D4-4B054E3F0959')

" );

        }

        /// <summary>
        /// Operations to be performed during the downgrade process.
        /// </summary>
        public override void Down()
        {
            // Attrib for BlockType: com_ccvonline - Residency - Resident Project Assessment List:Show Grid Title
            DeleteAttribute( "F5DAD59C-AC05-4BCA-ACC3-171924039872" );
            // Attrib for BlockType: com_ccvonline - Residency - Resident Project Assessment List:Show Project Column
            DeleteAttribute( "F0390AAB-D114-4367-8349-35EEA8EDACB8" );
            // Attrib for BlockType: com_ccvonline - Residency - Resident Project Assessment List:Show Competency Column
            DeleteAttribute( "BFE19B2F-D62D-4C87-B928-FFFAE65BD12E" );

            DeleteBlock( "1DB06C26-B318-46CD-9E9F-219FC1EF6338" ); // Resident Project Assessment Detail
            DeleteBlock( "B459F23A-9C32-4537-BA93-637A81ACB35A" ); // Resident Project Assessment List
            DeleteBlock( "3938E111-C9FF-49E9-B1B8-A2AA89080F51" ); // Page Xslt Transformation
            DeleteBlock( "4EFF1322-6A9A-44A0-B3B8-CB547CB09C0B" ); // Current Person
            DeleteBlock( "D07780FC-0ED5-4881-8B76-24F6FAE8A897" ); // Page Xslt Transformation
            DeleteBlock( "E517DDD7-73DB-4475-87A4-83CBCD7657F1" ); // Current Person
            DeletePage( "BDA4C473-01CD-449E-97D4-4B054E3F0959" ); // Project Assessment Detail
            DeletePage( "162927F6-E503-43C4-B075-55F1E592E96E" ); // Assessments
        }
    }
}
/* Skipped Operations for tables that are not part of ResidencyContext: Review these comments to verify the proper things were skipped */
/* To disable skipping, edit your Migrations\Configuration.cs so that CodeGenerator = new RockCSharpMigrationCodeGenerator<ResidencyContext>(false); */

// Up()...
// AddColumnOperation for TableName BinaryFile, column StorageEntityTypeId.
// AddColumnOperation for TableName BinaryFileType, column StorageEntityTypeId.
// AddColumnOperation for TableName Campus, column ShortCode.
// CreateIndexOperation for TableName BinaryFileType, column StorageEntityTypeId.
// CreateIndexOperation for TableName BinaryFile, column StorageEntityTypeId.
// AddForeignKeyOperation for TableName BinaryFileType, column StorageEntityTypeId.
// AddForeignKeyOperation for TableName BinaryFile, column StorageEntityTypeId.

// Down()...
// DropForeignKeyOperation for TableName BinaryFile, column StorageEntityTypeId.
// DropForeignKeyOperation for TableName BinaryFileType, column StorageEntityTypeId.
// DropIndexOperation for TableName BinaryFile, column StorageEntityTypeId.
// DropIndexOperation for TableName BinaryFileType, column StorageEntityTypeId.
// DropColumnOperation for TableName Campus, column ShortCode.
// DropColumnOperation for TableName BinaryFileType, column StorageEntityTypeId.
// DropColumnOperation for TableName BinaryFile, column StorageEntityTypeId.
