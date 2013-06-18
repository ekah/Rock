namespace com.ccvonline.Residency.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    /// <summary>
    /// 
    /// </summary>
    public partial class BlocksPages01 : Rock.Migrations.RockMigration
    {
        /// <summary>
        /// Operations to be performed during the upgrade process.
        /// </summary>
        public override void Up()
        {
            AddPage( "20F97A93-7949-4C2A-8A5E-C756FE8585CA", "Residency", "", "Default", "82B81403-8A93-4F42-A958-5303C3AF1508", "icon-user-md" );
            AddPage( "82B81403-8A93-4F42-A958-5303C3AF1508", "Configuration", "Configure various aspects of the Residency application", "Default", "FD705EED-CD8D-4F53-8C16-ABBA15CC27D5", "" );
            AddPage( "FD705EED-CD8D-4F53-8C16-ABBA15CC27D5", "Periods", "", "Default", "4B507217-5C12-4479-B5CD-B696B1445653", "" );
            AddPage( "4B507217-5C12-4479-B5CD-B696B1445653", "Period Detail", "", "Default", "F8D8663B-FE4F-4F48-A359-DBE656AE69A2", "" );
            AddPage( "F8D8663B-FE4F-4F48-A359-DBE656AE69A2", "Residency Track", "", "Default", "038AEF17-65EE-4161-BF9E-64AACC791701", "" );
            AddPage( "038AEF17-65EE-4161-BF9E-64AACC791701", "Competency Detail", "", "Default", "2BD2E7BB-4199-4C18-B51A-AA3755DECD1B", "" );
            AddPage( "2BD2E7BB-4199-4C18-B51A-AA3755DECD1B", "Project Detail", "", "Default", "37BA8EAD-16C5-4257-953D-D202684A8E61", "" );
            AddPage( "37BA8EAD-16C5-4257-953D-D202684A8E61", "Point of Assessment Detail", "", "Default", "DD65505A-6FE2-4478-8901-9F38F484E3EB", "" );

            AddBlockType( "com .ccvonline - Residency Period List", "", "~/Plugins/com.ccvonline/ResidencyPeriodList.ascx", "81C5EE50-AE8D-45F9-8014-A7C65F0FDBD6" );
            
            AddBlockType( "com .ccvonline - Residency Period Detail", "", "~/Plugins/com.ccvonline/ResidencyPeriodDetail.ascx", "511421DB-E127-447D-81A6-FF8C52D11815" );
            AddBlockType( "com .ccvonline - Residency Track List", "", "~/Plugins/com.ccvonline/ResidencyTrackList.ascx", "A3E2F4B9-FC87-472A-B873-2BB649C2417B" );
            
            AddBlockType( "com .ccvonline - Residency Track Detail", "", "~/Plugins/com.ccvonline/ResidencyTrackDetail.ascx", "72133176-4E1A-4851-B4D0-BBC447D84440" );
            AddBlockType( "com .ccvonline - Residency Competency List", "", "~/Plugins/com.ccvonline/ResidencyCompetencyList.ascx", "488BB996-8F4B-4DB4-9B0B-FB7B959BCDAF" );
            
            AddBlockType( "com .ccvonline - Residency Competency Detail", "", "~/Plugins/com.ccvonline/ResidencyCompetencyDetail.ascx", "D1D1C418-B84B-4307-B4EC-D2FD2970D639" );
            AddBlockType( "com .ccvonline - Residency Project List", "", "~/Plugins/com.ccvonline/ResidencyProjectList.ascx", "1A1E32B5-93BC-480E-BFB4-A5D9C06DCBF2" );
            
            AddBlockType( "com .ccvonline - Residency Project Detail", "", "~/Plugins/com.ccvonline/ResidencyProjectDetail.ascx", "8BA15032-D16A-4FDC-AE7F-A77F50267F39" );
            AddBlockType( "com .ccvonline - Residency Project Point Of Assessment List", "", "~/Plugins/com.ccvonline/ResidencyProjectPointOfAssessmentList.ascx", "8EEE930E-F879-48DC-8AFB-7249B618034D" );
            
            AddBlockType( "com .ccvonline - Residency Project Point Of Assessment Detail", "", "~/Plugins/com.ccvonline/ResidencyProjectPointOfAssessmentDetail.ascx", "A56E3BE8-AB33-4CEA-9C93-F138B7E24498" );
            
            AddBlock( "4B507217-5C12-4479-B5CD-B696B1445653", "81C5EE50-AE8D-45F9-8014-A7C65F0FDBD6", "Periods", "", "Content", 0, "856C9158-08F9-4A89-9AE4-124109DA6A1E" );
            
            AddBlock( "F8D8663B-FE4F-4F48-A359-DBE656AE69A2", "511421DB-E127-447D-81A6-FF8C52D11815", "Period Detail", "", "Content", 0, "F868F454-D163-4F35-9768-CCAC14908D83" );
            AddBlock( "F8D8663B-FE4F-4F48-A359-DBE656AE69A2", "A3E2F4B9-FC87-472A-B873-2BB649C2417B", "Residency Track List", "", "Content", 1, "45B63A50-F7DD-419A-BF8E-97969C193A47" );
            
            AddBlock( "038AEF17-65EE-4161-BF9E-64AACC791701", "72133176-4E1A-4851-B4D0-BBC447D84440", "Residency Track Detail", "", "Content", 0, "92F53B4E-1817-4BA6-A673-47DB3DE17722" );
            AddBlock( "038AEF17-65EE-4161-BF9E-64AACC791701", "488BB996-8F4B-4DB4-9B0B-FB7B959BCDAF", "Residency Competency List", "", "Content", 1, "286A833E-0A5A-4FAB-ACFA-71CCEEEC1AB4" );
            
            AddBlock( "2BD2E7BB-4199-4C18-B51A-AA3755DECD1B", "D1D1C418-B84B-4307-B4EC-D2FD2970D639", "Residency Competency Detail", "", "Content", 0, "59C12E5C-7478-4E24-843C-14561C47FBD1" );
            AddBlock( "2BD2E7BB-4199-4C18-B51A-AA3755DECD1B", "1A1E32B5-93BC-480E-BFB4-A5D9C06DCBF2", "Residency Project List", "", "Content", 1, "F075238F-E4E2-4291-8F2B-7EB0ACD5888D" );
            
            AddBlock( "37BA8EAD-16C5-4257-953D-D202684A8E61", "8BA15032-D16A-4FDC-AE7F-A77F50267F39", "Residency Project Detail", "", "Content", 0, "73C4692B-7A54-48CE-9611-4B3E4ABB9EA9" );
            AddBlock( "37BA8EAD-16C5-4257-953D-D202684A8E61", "8EEE930E-F879-48DC-8AFB-7249B618034D", "Residency Project Point Of Assessment List", "", "Content", 1, "986909E7-D5C4-47BF-AE2E-ED93C2D915A1" );
            
            AddBlock( "DD65505A-6FE2-4478-8901-9F38F484E3EB", "A56E3BE8-AB33-4CEA-9C93-F138B7E24498", "Residency Project Point Of Assessment Detail", "", "Content", 0, "4C4223CF-B656-4CF1-9319-C2350D0E9A7D" );

            // Attrib for BlockType: com .ccvonline - Residency Period List:Detail Page
            AddBlockTypeAttribute( "81C5EE50-AE8D-45F9-8014-A7C65F0FDBD6", "BD53F9C9-EBA9-4D3F-82EA-DE5DD34A8108", "Detail Page", "DetailPageGuid", "", "", 0, "", "55B21688-D933-4C78-9F78-76B965BD1C3F" );

            // Attrib for BlockType: com .ccvonline - Residency Track List:Detail Page
            AddBlockTypeAttribute( "A3E2F4B9-FC87-472A-B873-2BB649C2417B", "BD53F9C9-EBA9-4D3F-82EA-DE5DD34A8108", "Detail Page", "DetailPageGuid", "", "", 0, "", "9AF9C65C-F6E1-425C-9AAC-C9BDF988B1F3" );

            // Attrib for BlockType: com .ccvonline - Residency Competency List:Detail Page
            AddBlockTypeAttribute( "488BB996-8F4B-4DB4-9B0B-FB7B959BCDAF", "BD53F9C9-EBA9-4D3F-82EA-DE5DD34A8108", "Detail Page", "DetailPageGuid", "", "", 0, "", "69538108-46CD-4E29-9701-414E85E7BA0D" );

            // Attrib for BlockType: com .ccvonline - Residency Project List:Detail Page
            AddBlockTypeAttribute( "1A1E32B5-93BC-480E-BFB4-A5D9C06DCBF2", "BD53F9C9-EBA9-4D3F-82EA-DE5DD34A8108", "Detail Page", "DetailPageGuid", "", "", 0, "", "9CA38214-F938-4800-BCB6-8158C548FDD0" );

            // Attrib for BlockType: com .ccvonline - Residency Project Point Of Assessment List:Detail Page
            AddBlockTypeAttribute( "8EEE930E-F879-48DC-8AFB-7249B618034D", "BD53F9C9-EBA9-4D3F-82EA-DE5DD34A8108", "Detail Page", "DetailPageGuid", "", "", 0, "", "4089261C-A3B9-451F-AEB1-57B5458B3EEB" );

            // Attrib for BlockType: com .ccvonline - Residency Project Detail:Residency Competency Page
            AddBlockTypeAttribute( "8BA15032-D16A-4FDC-AE7F-A77F50267F39", "BD53F9C9-EBA9-4D3F-82EA-DE5DD34A8108", "Residency Competency Page", "ResidencyCompetencyPage", "", "", 0, "", "583DDD83-2D7C-4CA3-886F-98628DAD575D" );

            // Attrib Value for Periods:Detail Page
            AddBlockAttributeValue( "856C9158-08F9-4A89-9AE4-124109DA6A1E", "55B21688-D933-4C78-9F78-76B965BD1C3F", "f8d8663b-fe4f-4f48-a359-dbe656ae69a2" );

            // Attrib for BlockType: com .ccvonline - Residency Track Detail:Residency Period Page
            AddBlockTypeAttribute( "72133176-4E1A-4851-B4D0-BBC447D84440", "BD53F9C9-EBA9-4D3F-82EA-DE5DD34A8108", "Residency Period Page", "ResidencyPeriodPage", "", "", 0, "", "AAD879A5-B1E9-420D-B4CC-E1780D1D7B06" );

            // Attrib Value for Residency Track List:Detail Page
            AddBlockAttributeValue( "45B63A50-F7DD-419A-BF8E-97969C193A47", "9AF9C65C-F6E1-425C-9AAC-C9BDF988B1F3", "038aef17-65ee-4161-bf9e-64aacc791701" );

            // Attrib Value for Residency Competency List:Detail Page
            AddBlockAttributeValue( "286A833E-0A5A-4FAB-ACFA-71CCEEEC1AB4", "69538108-46CD-4E29-9701-414E85E7BA0D", "2bd2e7bb-4199-4c18-b51a-aa3755decd1b" );

            // Attrib Value for Residency Project List:Detail Page
            AddBlockAttributeValue( "F075238F-E4E2-4291-8F2B-7EB0ACD5888D", "9CA38214-F938-4800-BCB6-8158C548FDD0", "37ba8ead-16c5-4257-953d-d202684a8e61" );

            // Attrib Value for Residency Project Point Of Assessment List:Detail Page
            AddBlockAttributeValue( "986909E7-D5C4-47BF-AE2E-ED93C2D915A1", "4089261C-A3B9-451F-AEB1-57B5458B3EEB", "dd65505a-6fe2-4478-8901-9f38f484e3eb" );

            // Attrib Value for Residency Project Detail:Residency Competency Page
            AddBlockAttributeValue( "73C4692B-7A54-48CE-9611-4B3E4ABB9EA9", "583DDD83-2D7C-4CA3-886F-98628DAD575D", "2bd2e7bb-4199-4c18-b51a-aa3755decd1b" );

            // Attrib Value for Residency Track Detail:Residency Period Page
            AddBlockAttributeValue( "92F53B4E-1817-4BA6-A673-47DB3DE17722", "AAD879A5-B1E9-420D-B4CC-E1780D1D7B06", "f8d8663b-fe4f-4f48-a359-dbe656ae69a2" );
        }

        /// <summary>
        /// Operations to be performed during the downgrade process.
        /// </summary>
        public override void Down()
        {
            DeleteAttribute( "583DDD83-2D7C-4CA3-886F-98628DAD575D" ); // Residency Competency Page
            DeleteAttribute( "4089261C-A3B9-451F-AEB1-57B5458B3EEB" ); // Detail Page
            DeleteAttribute( "9CA38214-F938-4800-BCB6-8158C548FDD0" ); // Detail Page
            DeleteAttribute( "69538108-46CD-4E29-9701-414E85E7BA0D" ); // Detail Page
            DeleteAttribute( "9AF9C65C-F6E1-425C-9AAC-C9BDF988B1F3" ); // Detail Page
            DeleteAttribute( "55B21688-D933-4C78-9F78-76B965BD1C3F" ); // Detail Page
            DeleteAttribute( "AAD879A5-B1E9-420D-B4CC-E1780D1D7B06" ); // Residency Period Page
            
            DeleteBlock( "4C4223CF-B656-4CF1-9319-C2350D0E9A7D" ); // Residency Project Point Of Assessment Detail
            DeleteBlock( "986909E7-D5C4-47BF-AE2E-ED93C2D915A1" ); // Residency Project Point Of Assessment List
            DeleteBlock( "73C4692B-7A54-48CE-9611-4B3E4ABB9EA9" ); // Residency Project Detail
            DeleteBlock( "F075238F-E4E2-4291-8F2B-7EB0ACD5888D" ); // Residency Project List
            DeleteBlock( "59C12E5C-7478-4E24-843C-14561C47FBD1" ); // Residency Competency Detail
            DeleteBlock( "286A833E-0A5A-4FAB-ACFA-71CCEEEC1AB4" ); // Residency Competency List
            DeleteBlock( "92F53B4E-1817-4BA6-A673-47DB3DE17722" ); // Residency Track Detail
            DeleteBlock( "45B63A50-F7DD-419A-BF8E-97969C193A47" ); // Residency Track List
            DeleteBlock( "F868F454-D163-4F35-9768-CCAC14908D83" ); // Period Detail
            DeleteBlock( "856C9158-08F9-4A89-9AE4-124109DA6A1E" ); // Periods
            
            DeleteBlockType( "A56E3BE8-AB33-4CEA-9C93-F138B7E24498" ); // com .ccvonline - Residency Project Point Of Assessment Detail
            DeleteBlockType( "8EEE930E-F879-48DC-8AFB-7249B618034D" ); // com .ccvonline - Residency Project Point Of Assessment List
            DeleteBlockType( "1A1E32B5-93BC-480E-BFB4-A5D9C06DCBF2" ); // com .ccvonline - Residency Project List
            DeleteBlockType( "8BA15032-D16A-4FDC-AE7F-A77F50267F39" ); // com .ccvonline - Residency Project Detail
            DeleteBlockType( "488BB996-8F4B-4DB4-9B0B-FB7B959BCDAF" ); // com .ccvonline - Residency Competency List
            DeleteBlockType( "D1D1C418-B84B-4307-B4EC-D2FD2970D639" ); // com .ccvonline - Residency Competency Detail
            DeleteBlockType( "72133176-4E1A-4851-B4D0-BBC447D84440" ); // com .ccvonline - Residency Track Detail
            DeleteBlockType( "A3E2F4B9-FC87-472A-B873-2BB649C2417B" ); // com .ccvonline - Residency Track List
            DeleteBlockType( "511421DB-E127-447D-81A6-FF8C52D11815" ); // com .ccvonline - Residency Period Detail
            DeleteBlockType( "81C5EE50-AE8D-45F9-8014-A7C65F0FDBD6" ); // com .ccvonline - Residency Period List
            
            DeletePage( "DD65505A-6FE2-4478-8901-9F38F484E3EB" ); // Point of Assessment Detail
            DeletePage( "37BA8EAD-16C5-4257-953D-D202684A8E61" ); // Project Detail
            DeletePage( "2BD2E7BB-4199-4C18-B51A-AA3755DECD1B" ); // Competency Detail
            DeletePage( "038AEF17-65EE-4161-BF9E-64AACC791701" ); // Residency Track
            DeletePage( "F8D8663B-FE4F-4F48-A359-DBE656AE69A2" ); // Period Detail
            DeletePage( "4B507217-5C12-4479-B5CD-B696B1445653" ); // Periods
            DeletePage( "FD705EED-CD8D-4F53-8C16-ABBA15CC27D5" ); // Configuration
            DeletePage( "82B81403-8A93-4F42-A958-5303C3AF1508" ); // Residency
        }
    }
}
