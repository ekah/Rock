namespace com.ccvonline.Residency.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class BlocksPages01 : Rock.Migrations.RockMigration
    {
        public override void Up()
        {
            AddPage( "20F97A93-7949-4C2A-8A5E-C756FE8585CA", "Residency", "", "Default", "82B81403-8A93-4F42-A958-5303C3AF1508", "icon-user-md" );
            AddPage( "82B81403-8A93-4F42-A958-5303C3AF1508", "Configuration", "Configure various aspects of the Residency application", "Default", "FD705EED-CD8D-4F53-8C16-ABBA15CC27D5", "" );
            AddPage( "FD705EED-CD8D-4F53-8C16-ABBA15CC27D5", "Periods", "", "Default", "4B507217-5C12-4479-B5CD-B696B1445653", "" );
            AddPage( "4B507217-5C12-4479-B5CD-B696B1445653", "Period Detail", "", "Default", "F8D8663B-FE4F-4F48-A359-DBE656AE69A2", "" );
            AddPage( "FD705EED-CD8D-4F53-8C16-ABBA15CC27D5", "Tracks", "", "Default", "9574192B-8DC5-4B9F-BFF7-27064871E16F", "" );
            AddPage( "FD705EED-CD8D-4F53-8C16-ABBA15CC27D5", "Competencies", "", "Default", "C022D01B-A5B6-4B6A-8CF7-F6ED24B4FCA7", "" );
            AddPage( "FD705EED-CD8D-4F53-8C16-ABBA15CC27D5", "Projects", "", "Default", "3DF6FE23-E623-42AD-8A19-29D7F5A10D39", "" );
            AddPage( "9574192B-8DC5-4B9F-BFF7-27064871E16F", "Track Detail", "", "Default", "9EB986E6-4FD9-416D-8375-DDB0ABB17D9E", "" );
            AddPage( "C022D01B-A5B6-4B6A-8CF7-F6ED24B4FCA7", "Competency Detail", "", "Default", "18CE112E-A8DF-4CF8-B9E3-DFF5C9A88C4C", "" );
            AddPage( "3DF6FE23-E623-42AD-8A19-29D7F5A10D39", "Project Detail", "", "Default", "42571EEA-25D6-4DDD-BDFA-7E5220F721BE", "" );
            AddPage( "42571EEA-25D6-4DDD-BDFA-7E5220F721BE", "Residency Project Point of Assessment Detail", "", "Default", "A18DD129-4DC1-485F-BCB8-BE5D2B7E9D72", "" );
            AddPage( "18CE112E-A8DF-4CF8-B9E3-DFF5C9A88C4C", "Competency Project", "", "Default", "8D0C8656-E47A-45C0-B0C6-39F8ED3BF8FE", "" );
            AddPage( "8D0C8656-E47A-45C0-B0C6-39F8ED3BF8FE", "Competency Project Point Of Assessment", "", "Default", "D2138591-5703-4F2A-BDEC-1EB3FAB5F7AB", "" );
            AddBlockType( "com .ccvonline - Residency Period List", "", "~/Plugins/com.ccvonline/ResidencyPeriodList.ascx", "81C5EE50-AE8D-45F9-8014-A7C65F0FDBD6" );
            AddBlockType( "com .ccvonline - Residency Period Detail", "", "~/Plugins/com.ccvonline/ResidencyPeriodDetail.ascx", "511421DB-E127-447D-81A6-FF8C52D11815" );
            AddBlockType( "com .ccvonline - Residency Track List", "", "~/Plugins/com.ccvonline/ResidencyTrackList.ascx", "A3E2F4B9-FC87-472A-B873-2BB649C2417B" );
            AddBlockType( "com .ccvonline - Residency Track Detail", "", "~/Plugins/com.ccvonline/ResidencyTrackDetail.ascx", "72133176-4E1A-4851-B4D0-BBC447D84440" );
            AddBlockType( "com .ccvonline - Residency Competency Detail", "", "~/Plugins/com.ccvonline/ResidencyCompetencyDetail.ascx", "D1D1C418-B84B-4307-B4EC-D2FD2970D639" );
            AddBlockType( "com .ccvonline - Residency Competency List", "", "~/Plugins/com.ccvonline/ResidencyCompetencyList.ascx", "488BB996-8F4B-4DB4-9B0B-FB7B959BCDAF" );
            AddBlockType( "com .ccvonline - Residency Project Detail", "", "~/Plugins/com.ccvonline/ResidencyProjectDetail.ascx", "8BA15032-D16A-4FDC-AE7F-A77F50267F39" );
            AddBlockType( "com .ccvonline - Residency Project List", "", "~/Plugins/com.ccvonline/ResidencyProjectList.ascx", "1A1E32B5-93BC-480E-BFB4-A5D9C06DCBF2" );
            AddBlockType( "com .ccvonline - Residency Project Point Of Assessment List", "", "~/Plugins/com.ccvonline/ResidencyProjectPointOfAssessmentList.ascx", "8EEE930E-F879-48DC-8AFB-7249B618034D" );
            AddBlockType( "com .ccvonline - Residency Project Point Of Assessment Detail", "", "~/Plugins/com.ccvonline/ResidencyProjectPointOfAssessmentDetail.ascx", "A56E3BE8-AB33-4CEA-9C93-F138B7E24498" );
            AddBlock( "4B507217-5C12-4479-B5CD-B696B1445653", "81C5EE50-AE8D-45F9-8014-A7C65F0FDBD6", "Periods", "", "Content", 0, "856C9158-08F9-4A89-9AE4-124109DA6A1E" );
            AddBlock( "F8D8663B-FE4F-4F48-A359-DBE656AE69A2", "511421DB-E127-447D-81A6-FF8C52D11815", "Period Detail", "", "Content", 0, "F868F454-D163-4F35-9768-CCAC14908D83" );
            AddBlock( "9574192B-8DC5-4B9F-BFF7-27064871E16F", "A3E2F4B9-FC87-472A-B873-2BB649C2417B", "Tracks", "", "Content", 0, "70C583A3-99B3-4FD8-9D40-7BF1CADC0210" );
            AddBlock( "9EB986E6-4FD9-416D-8375-DDB0ABB17D9E", "72133176-4E1A-4851-B4D0-BBC447D84440", "Residency Track Detail", "", "Content", 0, "5C5D28E1-705C-4677-82C4-CE6EBBDACBD0" );
            AddBlock( "C022D01B-A5B6-4B6A-8CF7-F6ED24B4FCA7", "488BB996-8F4B-4DB4-9B0B-FB7B959BCDAF", "Residency Competency List", "", "Content", 0, "8DA855BC-DA31-470D-A9A2-155B4103B68D" );
            AddBlock( "18CE112E-A8DF-4CF8-B9E3-DFF5C9A88C4C", "D1D1C418-B84B-4307-B4EC-D2FD2970D639", "Residency Competency Detail", "", "Content", 0, "9BADFF8C-43E5-4D20-AE01-8A0AB5454038" );
            AddBlock( "3DF6FE23-E623-42AD-8A19-29D7F5A10D39", "1A1E32B5-93BC-480E-BFB4-A5D9C06DCBF2", "Residency Project List", "", "Content", 0, "DB553513-AF33-486A-B61F-8DE9534ED42C" );
            AddBlock( "42571EEA-25D6-4DDD-BDFA-7E5220F721BE", "8BA15032-D16A-4FDC-AE7F-A77F50267F39", "Residency Project Detail", "", "Content", 0, "10F5310C-FBEF-42A1-83AB-61CDFCFCFB35" );
            AddBlock( "18CE112E-A8DF-4CF8-B9E3-DFF5C9A88C4C", "1A1E32B5-93BC-480E-BFB4-A5D9C06DCBF2", "Residency Project List", "", "Content", 1, "9E96971C-8AFA-47BE-BBFA-AD78453900B8" );
            AddBlock( "42571EEA-25D6-4DDD-BDFA-7E5220F721BE", "8EEE930E-F879-48DC-8AFB-7249B618034D", "Residency Project Point Of Assessment List", "", "Content", 1, "F1F5D3FE-F41D-4D1C-95C4-B7A1769BF7DD" );
            AddBlock( "A18DD129-4DC1-485F-BCB8-BE5D2B7E9D72", "A56E3BE8-AB33-4CEA-9C93-F138B7E24498", "Residency Project Point Of Assessment Detail", "", "Content", 0, "6066710E-63BA-4FA9-911D-832424CD2D33" );
            AddBlock( "8D0C8656-E47A-45C0-B0C6-39F8ED3BF8FE", "8BA15032-D16A-4FDC-AE7F-A77F50267F39", "Residency Project Detail", "", "Content", 0, "8653EB9A-94EC-41B5-9BFD-4D0C064E524A" );
            AddBlock( "8D0C8656-E47A-45C0-B0C6-39F8ED3BF8FE", "8EEE930E-F879-48DC-8AFB-7249B618034D", "Residency Project Point Of Assessment List", "", "Content", 1, "336D02FE-3D5E-4147-9689-FE3460277757" );
            AddBlock( "D2138591-5703-4F2A-BDEC-1EB3FAB5F7AB", "A56E3BE8-AB33-4CEA-9C93-F138B7E24498", "Residency Project Point Of Assessment Detail", "", "Content", 0, "569BD9CB-253D-4EFA-8B70-0ECF275609C4" );
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

            // Attrib Value for Tracks:Detail Page
            AddBlockAttributeValue( "70C583A3-99B3-4FD8-9D40-7BF1CADC0210", "9AF9C65C-F6E1-425C-9AAC-C9BDF988B1F3", "9eb986e6-4fd9-416d-8375-ddb0abb17d9e" );

            // Attrib Value for Residency Competency List:Detail Page
            AddBlockAttributeValue( "8DA855BC-DA31-470D-A9A2-155B4103B68D", "69538108-46CD-4E29-9701-414E85E7BA0D", "18ce112e-a8df-4cf8-b9e3-dff5c9a88c4c" );

            // Attrib Value for Residency Project List:Detail Page
            AddBlockAttributeValue( "DB553513-AF33-486A-B61F-8DE9534ED42C", "9CA38214-F938-4800-BCB6-8158C548FDD0", "42571eea-25d6-4ddd-bdfa-7e5220f721be" );

            // Attrib Value for Residency Project List:Detail Page
            AddBlockAttributeValue( "9E96971C-8AFA-47BE-BBFA-AD78453900B8", "9CA38214-F938-4800-BCB6-8158C548FDD0", "8d0c8656-e47a-45c0-b0c6-39f8ed3bf8fe" );

            // Attrib Value for Residency Project Point Of Assessment List:Detail Page
            AddBlockAttributeValue( "F1F5D3FE-F41D-4D1C-95C4-B7A1769BF7DD", "4089261C-A3B9-451F-AEB1-57B5458B3EEB", "a18dd129-4dc1-485f-bcb8-be5d2b7e9d72" );

            // Attrib Value for Residency Project Point Of Assessment List:Detail Page
            AddBlockAttributeValue( "336D02FE-3D5E-4147-9689-FE3460277757", "4089261C-A3B9-451F-AEB1-57B5458B3EEB", "d2138591-5703-4f2a-bdec-1eb3fab5f7ab" );

            // Attrib Value for Residency Project Detail:Residency Competency Page
            AddBlockAttributeValue( "10F5310C-FBEF-42A1-83AB-61CDFCFCFB35", "583DDD83-2D7C-4CA3-886F-98628DAD575D", "18ce112e-a8df-4cf8-b9e3-dff5c9a88c4c" );
        }
        
        public override void Down()
        {
            DeleteAttribute( "583DDD83-2D7C-4CA3-886F-98628DAD575D" ); // Residency Competency Page
            DeleteAttribute( "4089261C-A3B9-451F-AEB1-57B5458B3EEB" ); // Detail Page
            DeleteAttribute( "9CA38214-F938-4800-BCB6-8158C548FDD0" ); // Detail Page
            DeleteAttribute( "69538108-46CD-4E29-9701-414E85E7BA0D" ); // Detail Page
            DeleteAttribute( "9AF9C65C-F6E1-425C-9AAC-C9BDF988B1F3" ); // Detail Page
            DeleteAttribute( "55B21688-D933-4C78-9F78-76B965BD1C3F" ); // Detail Page
            DeleteBlock( "569BD9CB-253D-4EFA-8B70-0ECF275609C4" ); // Residency Project Point Of Assessment Detail
            DeleteBlock( "336D02FE-3D5E-4147-9689-FE3460277757" ); // Residency Project Point Of Assessment List
            DeleteBlock( "8653EB9A-94EC-41B5-9BFD-4D0C064E524A" ); // Residency Project Detail
            DeleteBlock( "6066710E-63BA-4FA9-911D-832424CD2D33" ); // Residency Project Point Of Assessment Detail
            DeleteBlock( "F1F5D3FE-F41D-4D1C-95C4-B7A1769BF7DD" ); // Residency Project Point Of Assessment List
            DeleteBlock( "9E96971C-8AFA-47BE-BBFA-AD78453900B8" ); // Residency Project List
            DeleteBlock( "10F5310C-FBEF-42A1-83AB-61CDFCFCFB35" ); // Residency Project Detail
            DeleteBlock( "DB553513-AF33-486A-B61F-8DE9534ED42C" ); // Residency Project List
            DeleteBlock( "9BADFF8C-43E5-4D20-AE01-8A0AB5454038" ); // Residency Competency Detail
            DeleteBlock( "8DA855BC-DA31-470D-A9A2-155B4103B68D" ); // Residency Competency List
            DeleteBlock( "5C5D28E1-705C-4677-82C4-CE6EBBDACBD0" ); // Residency Track Detail
            DeleteBlock( "70C583A3-99B3-4FD8-9D40-7BF1CADC0210" ); // Tracks
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
            DeletePage( "D2138591-5703-4F2A-BDEC-1EB3FAB5F7AB" ); // Competency Project Point Of Assessment
            DeletePage( "8D0C8656-E47A-45C0-B0C6-39F8ED3BF8FE" ); // Competency Project
            DeletePage( "A18DD129-4DC1-485F-BCB8-BE5D2B7E9D72" ); // Residency Project Point of Assessment Detail
            DeletePage( "42571EEA-25D6-4DDD-BDFA-7E5220F721BE" ); // Project Detail
            DeletePage( "18CE112E-A8DF-4CF8-B9E3-DFF5C9A88C4C" ); // Competency Detail
            DeletePage( "9EB986E6-4FD9-416D-8375-DDB0ABB17D9E" ); // Track Detail
            DeletePage( "3DF6FE23-E623-42AD-8A19-29D7F5A10D39" ); // Projects
            DeletePage( "C022D01B-A5B6-4B6A-8CF7-F6ED24B4FCA7" ); // Competencies
            DeletePage( "9574192B-8DC5-4B9F-BFF7-27064871E16F" ); // Tracks
            DeletePage( "F8D8663B-FE4F-4F48-A359-DBE656AE69A2" ); // Period Detail
            DeletePage( "4B507217-5C12-4479-B5CD-B696B1445653" ); // Periods
            DeletePage( "FD705EED-CD8D-4F53-8C16-ABBA15CC27D5" ); // Configuration
            DeletePage( "82B81403-8A93-4F42-A958-5303C3AF1508" ); // Residency
        }
    }
}
