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
    public partial class ResidentAdminPagesBlocks : Rock.Migrations.RockMigration
    {
        /// <summary>
        /// Operations to be performed during the upgrade process.
        /// </summary>
        public override void Up()
        {
            AddPage( "FD705EED-CD8D-4F53-8C16-ABBA15CC27D5", "Resident Groups", "", "Default", "36428AF8-7650-4047-B655-8D39F5EA10C5", "" );
            AddPage( "36428AF8-7650-4047-B655-8D39F5EA10C5", "Residents", "", "Default", "531C64E2-282B-4644-9619-319BDBAC627E", "" );
            AddPage( "531C64E2-282B-4644-9619-319BDBAC627E", "Resident Detail", "", "Default", "57F487E8-46E4-4F6E-B03E-96CC2C4BE185", "" );
            AddPage( "57F487E8-46E4-4F6E-B03E-96CC2C4BE185", "Resident Competency Detail", "", "Default", "6F095271-8060-4577-8E72-C0EE2389527C", "" );
            AddPage( "6F095271-8060-4577-8E72-C0EE2389527C", "Resident Project Detail", "", "Default", "39661338-971E-45EA-86C3-7A8A5D2DEA54", "" );
            AddPage( "39661338-971E-45EA-86C3-7A8A5D2DEA54", "Project Assignment Detail", "", "Default", "165E7CB7-E15A-4AC2-8383-567A593279F0", "" );
            AddPage( "165E7CB7-E15A-4AC2-8383-567A593279F0", "Resident Project Assignment Assessment Detail", "", "Default", "32E7BCDE-37BC-48B5-B0FE-AA784AF0425A", "" );
            AddPage( "32E7BCDE-37BC-48B5-B0FE-AA784AF0425A", "Project Assignment Point of Assessment", "", "Default", "69A714EB-1870-4516-AB4F-63ADF2100FEA", "" );

            AddBlockType( "com .ccvonline - Residency Person List", "", "~/Plugins/com.ccvonline/ResidencyPersonList.ascx", "13EE0E6A-BBC6-4D86-9226-E246CFBA11B2" );
            AddBlockType( "com .ccvonline - Residency Person Detail", "", "~/Plugins/com.ccvonline/ResidencyPersonDetail.ascx", "F0A0BE3A-DD15-468F-93A6-97C440DB8253" );
            AddBlockType( "com .ccvonline - Residency Competency Person Detail", "", "~/Plugins/com.ccvonline/ResidencyCompetencyPersonDetail.ascx", "9E3D6BF6-28EE-4902-B6CD-AC1C31A7B731" );
            AddBlockType( "com .ccvonline - Residency Competency Person List", "", "~/Plugins/com.ccvonline/ResidencyCompetencyPersonList.ascx", "E4A531AD-4FCF-449B-91AB-ACBF87D83881" );
            AddBlockType( "com .ccvonline - Residency Competency Person Project Assignment Assessment List", "", "~/Plugins/com.ccvonline/ResidencyCompetencyPersonProjectAssignmentAssessmentList.ascx", "0B86F65A-904B-4FEE-86BF-99E1C1A696F5" );
            AddBlockType( "com .ccvonline - Residency Competency Person Project Assignment Assessment Point Of Assessment List", "", "~/Plugins/com.ccvonline/ResidencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessmentList.ascx", "203C01AB-0EE6-4EE0-A934-EB0FAF426E9C" );
            AddBlockType( "com .ccvonline - Residency Competency Person Project Assignment Detail", "", "~/Plugins/com.ccvonline/ResidencyCompetencyPersonProjectAssignmentDetail.ascx", "8A5FB3E3-4147-4DE0-9CAE-20974ADD5E70" );
            AddBlockType( "com .ccvonline - Residency Competency Person Project Assignment List", "", "~/Plugins/com.ccvonline/ResidencyCompetencyPersonProjectAssignmentList.ascx", "9C373F55-7E44-4641-AA6B-A30E7F214F37" );
            AddBlockType( "com .ccvonline - Residency Competency Person Project List", "", "~/Plugins/com.ccvonline/ResidencyCompetencyPersonProjectList.ascx", "C947DC2C-76E1-4B69-8FC6-E9E3134B36C7" );
            AddBlockType( "com .ccvonline - Residency Competency Person Project Detail", "", "~/Plugins/com.ccvonline/ResidencyCompetencyPersonProjectDetail.ascx", "5847D528-98BB-487C-BE26-A8FF60F74033" );
            AddBlockType( "com .ccvonline - Residency Competency Person Project Assignment Assessment Detail", "", "~/Plugins/com.ccvonline/ResidencyCompetencyPersonProjectAssignmentAssessmentDetail.ascx", "9EEA2EF7-E49D-4CAB-B067-BE4ECB9FF376" );
            AddBlockType( "com .ccvonline - Residency Competency Person Project Assignment Assessment POA Detail", "", "~/Plugins/com.ccvonline/ResidencyCompetencyPersonProjectAssignmentAssessmentPOADetail.ascx", "6285726B-A1C3-407C-A1C6-B4D18943A799" );
            AddBlockType( "com .ccvonline - Residency Competency Person Project Assignment Assessment POA List", "", "~/Plugins/com.ccvonline/ResidencyCompetencyPersonProjectAssignmentAssessmentPOAList.ascx", "0E6DAF91-AE77-4196-91B5-D1E3CB3B2403" );
            AddBlockType( "com .ccvonline - Residency Competency Person Project Assignment Assessment Point Of Assessment De...", "", "~/Plugins/com.ccvonline/ResidencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessmentDetail.ascx", "A102FA19-85A6-4701-9C14-29F1A412FDC0" );

            AddBlock( "531C64E2-282B-4644-9619-319BDBAC627E", "13EE0E6A-BBC6-4D86-9226-E246CFBA11B2", "Residency Person List", "", "Content", 1, "A66E242C-D1E0-4F4E-9F6A-ED7484352BFE" );
            AddBlock( "57F487E8-46E4-4F6E-B03E-96CC2C4BE185", "F0A0BE3A-DD15-468F-93A6-97C440DB8253", "Residency Person Detail", "", "Content", 0, "9CEC61A6-BEA6-420B-AEE1-8424E33998CB" );
            AddBlock( "57F487E8-46E4-4F6E-B03E-96CC2C4BE185", "E4A531AD-4FCF-449B-91AB-ACBF87D83881", "Residency Competency Person List", "", "Content", 2, "3F70C055-32CF-4F18-BBBB-6A0C88C5BE39" );
            AddBlock( "6F095271-8060-4577-8E72-C0EE2389527C", "9E3D6BF6-28EE-4902-B6CD-AC1C31A7B731", "Residency Competency Person Detail", "", "Content", 0, "71D6DCB8-6CEC-4703-8088-9B3FC0DEE7A6" );
            AddBlock( "531C64E2-282B-4644-9619-319BDBAC627E", "582BEEA1-5B27-444D-BC0A-F60CEB053981", "Group Detail", "", "Content", 0, "746B438D-75E3-495A-9678-C3C14629511A" );
            AddBlock( "36428AF8-7650-4047-B655-8D39F5EA10C5", "3D7FB6BE-6BBD-49F7-96B4-96310AF3048A", "Group List", "", "Content", 0, "AD59F37C-97EC-4F07-A604-3AAF8270C737" );
            AddBlock( "6F095271-8060-4577-8E72-C0EE2389527C", "C947DC2C-76E1-4B69-8FC6-E9E3134B36C7", "Residency Competency Person Project List", "", "Content", 2, "2A39AD5B-3497-4B47-9A66-1CEA68FD848C" );
            AddBlock( "39661338-971E-45EA-86C3-7A8A5D2DEA54", "5847D528-98BB-487C-BE26-A8FF60F74033", "Residency Competency Person Project Detail", "", "Content", 0, "F377C5DC-0618-4259-AA32-B75959CBEC85" );
            AddBlock( "39661338-971E-45EA-86C3-7A8A5D2DEA54", "9C373F55-7E44-4641-AA6B-A30E7F214F37", "Residency Competency Person Project Assignment List", "", "Content", 1, "31FFF300-F890-4243-B2A1-437F69ACE986" );
            AddBlock( "165E7CB7-E15A-4AC2-8383-567A593279F0", "8A5FB3E3-4147-4DE0-9CAE-20974ADD5E70", "Residency Competency Person Project Assignment Detail", "", "Content", 0, "BF51E84E-F37A-48CC-AAFB-D5A4FDEFF79E" );
            AddBlock( "165E7CB7-E15A-4AC2-8383-567A593279F0", "0B86F65A-904B-4FEE-86BF-99E1C1A696F5", "Residency Competency Person Project Assignment Assessment List", "", "Content", 1, "8B03F77F-4EAB-410B-9761-9CFE09CC0D3F" );
            AddBlock( "32E7BCDE-37BC-48B5-B0FE-AA784AF0425A", "203C01AB-0EE6-4EE0-A934-EB0FAF426E9C", "Residency Competency Person Project Assignment Assessment Point Of Assessment List", "", "Content", 1, "E32DA215-9919-4245-9799-0328AAA7E564" );
            AddBlock( "32E7BCDE-37BC-48B5-B0FE-AA784AF0425A", "9EEA2EF7-E49D-4CAB-B067-BE4ECB9FF376", "Residency Competency Person Project Assignment Assessment Detail", "", "Content", 0, "F32573AA-09E0-41C7-B614-A0AB88D8BDCD" );
            AddBlock( "69A714EB-1870-4516-AB4F-63ADF2100FEA", "A102FA19-85A6-4701-9C14-29F1A412FDC0", "Residency Competency Person Project Assignment Assessment Point Of Assessment Detail", "", "Content", 0, "72768282-BDCE-49AD-B325-F272EEC9B1F4" );
            AddBlock( "57F487E8-46E4-4F6E-B03E-96CC2C4BE185", "AAE2E5C3-9279-4AB0-9682-F4D19519D678", "Group Member Detail", "", "Content", 1, "4F104290-3664-40A7-9F6F-F0083900C9C6" );

            // Attrib for BlockType: com .ccvonline - Residency Person List:Detail Page
            AddBlockTypeAttribute( "13EE0E6A-BBC6-4D86-9226-E246CFBA11B2", "BD53F9C9-EBA9-4D3F-82EA-DE5DD34A8108", "Detail Page", "DetailPageGuid", "", "", 0, "", "7B1CA61D-0E1A-44C9-9210-DF49D29EECF0" );

            // Attrib for BlockType: com .ccvonline - Residency Competency Person List:Detail Page
            AddBlockTypeAttribute( "E4A531AD-4FCF-449B-91AB-ACBF87D83881", "BD53F9C9-EBA9-4D3F-82EA-DE5DD34A8108", "Detail Page", "DetailPageGuid", "", "", 0, "", "6E623135-F71C-481A-9A75-CD77EE0D6D81" );

            // Attrib for BlockType: com .ccvonline - Residency Competency Person Project Assignment List:Detail Page
            AddBlockTypeAttribute( "9C373F55-7E44-4641-AA6B-A30E7F214F37", "BD53F9C9-EBA9-4D3F-82EA-DE5DD34A8108", "Detail Page", "DetailPageGuid", "", "", 0, "", "3176A7C9-831F-403B-A549-AF1E1B4BC3D9" );

            // Attrib for BlockType: com .ccvonline - Residency Competency Person Project List:Detail Page
            AddBlockTypeAttribute( "C947DC2C-76E1-4B69-8FC6-E9E3134B36C7", "BD53F9C9-EBA9-4D3F-82EA-DE5DD34A8108", "Detail Page", "DetailPageGuid", "", "", 0, "", "01FD5A4D-3A9C-4F98-8955-E131C3500B23" );

            // Attrib for BlockType: com .ccvonline - Residency Competency Person Project Detail:Residency Competency Person Page
            AddBlockTypeAttribute( "5847D528-98BB-487C-BE26-A8FF60F74033", "BD53F9C9-EBA9-4D3F-82EA-DE5DD34A8108", "Residency Competency Person Page", "ResidencyCompetencyPersonPage", "", "", 0, "", "A718D831-2126-415D-A147-E67B78F8419F" );

            // Attrib for BlockType: Group List:Show GroupType
            AddBlockTypeAttribute( "3D7FB6BE-6BBD-49F7-96B4-96310AF3048A", "1EDAFDED-DFE6-4334-B019-6EECBA89E05A", "Show GroupType", "ShowGroupType", "", "", 0, "True", "242DF3C9-4BE8-4216-9AAE-CA19B9B8FC91" );

            // Attrib for BlockType: Group List:Show IsSystem
            AddBlockTypeAttribute( "3D7FB6BE-6BBD-49F7-96B4-96310AF3048A", "1EDAFDED-DFE6-4334-B019-6EECBA89E05A", "Show IsSystem", "ShowIsSystem", "", "", 0, "True", "7CDBC199-D661-4FF8-B1FD-1F7F822F69BE" );

            // Attrib for BlockType: com .ccvonline - Residency Competency Person Project Assignment Assessment List:Detail Page
            AddBlockTypeAttribute( "0B86F65A-904B-4FEE-86BF-99E1C1A696F5", "BD53F9C9-EBA9-4D3F-82EA-DE5DD34A8108", "Detail Page", "DetailPageGuid", "", "", 0, "", "710536D1-2664-4F48-ABBF-4DAD54A2DABB" );

            // Attrib for BlockType: com .ccvonline - Residency Competency Person Project Assignment Assessment Point Of Assessment List:Detail Page
            AddBlockTypeAttribute( "203C01AB-0EE6-4EE0-A934-EB0FAF426E9C", "BD53F9C9-EBA9-4D3F-82EA-DE5DD34A8108", "Detail Page", "DetailPageGuid", "", "", 0, "", "669E2765-BE03-4616-A471-B87B86807AB7" );

            // Attrib for BlockType: com .ccvonline - Residency Competency Person Project Assignment Assessment Detail:Resident Project Assignment Page
            AddBlockTypeAttribute( "9EEA2EF7-E49D-4CAB-B067-BE4ECB9FF376", "BD53F9C9-EBA9-4D3F-82EA-DE5DD34A8108", "Resident Project Assignment Page", "ResidentProjectAssignmentPage", "", "", 0, "", "5766BAC4-C57F-4B9B-B41C-4E153FA27E2A" );

            // Attrib for BlockType: com .ccvonline - Residency Competency Person Project Assignment Assessment Point Of Assessment De...:Resident Project Assignment Assessment Page
            AddBlockTypeAttribute( "A102FA19-85A6-4701-9C14-29F1A412FDC0", "BD53F9C9-EBA9-4D3F-82EA-DE5DD34A8108", "Resident Project Assignment Assessment Page", "ResidentProjectAssignmentAssessmentPage", "", "", 0, "", "84E3242F-97D3-4281-9511-ADB2825AD158" );

            // Attrib for BlockType: com .ccvonline - Residency Competency Person Detail:Resident Detail Page
            AddBlockTypeAttribute( "9E3D6BF6-28EE-4902-B6CD-AC1C31A7B731", "BD53F9C9-EBA9-4D3F-82EA-DE5DD34A8108", "Resident Detail Page", "ResidentDetailPage", "", "", 0, "", "241A26B9-C86C-4132-AEF4-E6C61D31B9FB" );

            // Attrib for BlockType: com .ccvonline - Residency Competency Person Project Assignment Detail:Resident Project Detail Page
            AddBlockTypeAttribute( "8A5FB3E3-4147-4DE0-9CAE-20974ADD5E70", "BD53F9C9-EBA9-4D3F-82EA-DE5DD34A8108", "Resident Project Detail Page", "ResidentProjectDetailPage", "", "", 0, "", "9627D0D6-5DB2-4B3C-A155-22D18E9AC287" );

            // Attrib Value for Group List:Limit to Security Role Groups
            AddBlockAttributeValue( "AD59F37C-97EC-4F07-A604-3AAF8270C737", "1DAD66E3-8859-487E-8200-483C98DE2E07", "False" );

            // Attrib Value for Group List:Group Types
            AddBlockAttributeValue( "AD59F37C-97EC-4F07-A604-3AAF8270C737", "C3FD6CE3-D37F-4A53-B0D7-AB1B1F252324", "19" );

            // Attrib Value for Group List:Detail Page
            AddBlockAttributeValue( "AD59F37C-97EC-4F07-A604-3AAF8270C737", "8E57EC42-ABEE-4D35-B7FA-D8513880E8E4", "531c64e2-282b-4644-9619-319bdbac627e" );

            // Attrib Value for Group List:Show User Count
            AddBlockAttributeValue( "AD59F37C-97EC-4F07-A604-3AAF8270C737", "D7A5D717-6B3F-4033-B707-B92D81D402C2", "True" );

            // Attrib Value for Group List:Show Description
            AddBlockAttributeValue( "AD59F37C-97EC-4F07-A604-3AAF8270C737", "99AF141C-8F5F-4FB8-8748-837A4BFCFB94", "False" );

            // Attrib Value for Group List:Show Edit
            AddBlockAttributeValue( "AD59F37C-97EC-4F07-A604-3AAF8270C737", "0EC725C5-F6F7-47DC-ABC2-8A59B6033F45", "True" );

            // Attrib Value for Group List:Show Notification
            AddBlockAttributeValue( "AD59F37C-97EC-4F07-A604-3AAF8270C737", "D5B9A3DB-DD94-4B7C-A784-28BA691181E0", "False" );

            // Attrib Value for Group Detail:Show Edit
            AddBlockAttributeValue( "746B438D-75E3-495A-9678-C3C14629511A", "50C7E223-459E-4A1C-AE3C-2892CBD40D22", "True" );

            // Attrib Value for Group Detail:Group Types
            AddBlockAttributeValue( "746B438D-75E3-495A-9678-C3C14629511A", "15AC7A62-7BF2-44B7-93CD-EA8F96BF529A", "19" );

            // Attrib Value for Group Detail:Limit to Security Role Groups
            AddBlockAttributeValue( "746B438D-75E3-495A-9678-C3C14629511A", "12295C7E-08F4-4AC5-8A34-C829620FC0B1", "False" );

            // Attrib Value for Residency Person List:Detail Page
            AddBlockAttributeValue( "A66E242C-D1E0-4F4E-9F6A-ED7484352BFE", "7B1CA61D-0E1A-44C9-9210-DF49D29EECF0", "57f487e8-46e4-4f6e-b03e-96cc2c4be185" );

            // Attrib Value for Residency Competency Person List:Detail Page
            AddBlockAttributeValue( "3F70C055-32CF-4F18-BBBB-6A0C88C5BE39", "6E623135-F71C-481A-9A75-CD77EE0D6D81", "6f095271-8060-4577-8e72-c0ee2389527c" );

            // Attrib Value for Residency Competency Person Project Assignment List:Detail Page
            AddBlockAttributeValue( "31FFF300-F890-4243-B2A1-437F69ACE986", "3176A7C9-831F-403B-A549-AF1E1B4BC3D9", "165e7cb7-e15a-4ac2-8383-567a593279f0" );

            // Attrib Value for Residency Competency Person Project List:Detail Page
            AddBlockAttributeValue( "2A39AD5B-3497-4B47-9A66-1CEA68FD848C", "01FD5A4D-3A9C-4F98-8955-E131C3500B23", "39661338-971e-45ea-86c3-7a8a5d2dea54" );

            // Attrib Value for Residency Competency Person Project Detail:Residency Competency Person Page
            AddBlockAttributeValue( "F377C5DC-0618-4259-AA32-B75959CBEC85", "A718D831-2126-415D-A147-E67B78F8419F", "6f095271-8060-4577-8e72-c0ee2389527c" );

            // Attrib Value for Group List:Show GroupType
            AddBlockAttributeValue( "AD59F37C-97EC-4F07-A604-3AAF8270C737", "242DF3C9-4BE8-4216-9AAE-CA19B9B8FC91", "False" );

            // Attrib Value for Group List:Show IsSystem
            AddBlockAttributeValue( "AD59F37C-97EC-4F07-A604-3AAF8270C737", "7CDBC199-D661-4FF8-B1FD-1F7F822F69BE", "False" );

            // Attrib Value for Residency Competency Person Project Assignment Assessment List:Detail Page
            AddBlockAttributeValue( "8B03F77F-4EAB-410B-9761-9CFE09CC0D3F", "710536D1-2664-4F48-ABBF-4DAD54A2DABB", "32e7bcde-37bc-48b5-b0fe-aa784af0425a" );

            // Attrib Value for Residency Competency Person Project Assignment Assessment Point Of Assessment List:Detail Page
            AddBlockAttributeValue( "E32DA215-9919-4245-9799-0328AAA7E564", "669E2765-BE03-4616-A471-B87B86807AB7", "69a714eb-1870-4516-ab4f-63adf2100fea" );

            // Attrib Value for Residency Competency Person Project Assignment Assessment Detail:Resident Project Assignment Page
            AddBlockAttributeValue( "F32573AA-09E0-41C7-B614-A0AB88D8BDCD", "5766BAC4-C57F-4B9B-B41C-4E153FA27E2A", "165e7cb7-e15a-4ac2-8383-567a593279f0" );

            // Attrib Value for Residency Competency Person Detail:Resident Detail Page
            AddBlockAttributeValue( "71D6DCB8-6CEC-4703-8088-9B3FC0DEE7A6", "241A26B9-C86C-4132-AEF4-E6C61D31B9FB", "57f487e8-46e4-4f6e-b03e-96cc2c4be185" );

            // Attrib Value for Residency Competency Person Project Assignment Detail:Resident Project Detail Page
            AddBlockAttributeValue( "BF51E84E-F37A-48CC-AAFB-D5A4FDEFF79E", "9627D0D6-5DB2-4B3C-A155-22D18E9AC287", "39661338-971e-45ea-86c3-7a8a5d2dea54" );

            // hide breadcrumb name for most of the resident admin pages
            Sql( @"
update [Page] set [BreadCrumbDisplayName] = 0 where [Guid] in 
  ('531C64E2-282B-4644-9619-319BDBAC627E','57F487E8-46E4-4F6E-B03E-96CC2C4BE185','6F095271-8060-4577-8E72-C0EE2389527C','39661338-971E-45EA-86C3-7A8A5D2DEA54','165E7CB7-E15A-4AC2-8383-567A593279F0','32E7BCDE-37BC-48B5-B0FE-AA784AF0425A','69A714EB-1870-4516-AB4F-63ADF2100FEA')
" );
        }

        /// <summary>
        /// Operations to be performed during the downgrade process.
        /// </summary>
        public override void Down()
        {
            // Attrib for BlockType: com .ccvonline - Residency Competency Person Project Assignment Detail:Resident Project Detail Page
            DeleteAttribute( "9627D0D6-5DB2-4B3C-A155-22D18E9AC287" );

            // Attrib for BlockType: com .ccvonline - Residency Competency Person Detail:Resident Detail Page
            DeleteAttribute( "241A26B9-C86C-4132-AEF4-E6C61D31B9FB" );

            // Attrib for BlockType: com .ccvonline - Residency Competency Person Project Assignment Assessment Point Of Assessment De...:Resident Project Assignment Assessment Page
            DeleteAttribute( "84E3242F-97D3-4281-9511-ADB2825AD158" );

            // Attrib for BlockType: com .ccvonline - Residency Competency Person Project Assignment Assessment Detail:Resident Project Assignment Page
            DeleteAttribute( "5766BAC4-C57F-4B9B-B41C-4E153FA27E2A" );

            // Attrib for BlockType: com .ccvonline - Residency Competency Person Project Assignment Assessment Point Of Assessment List:Detail Page
            DeleteAttribute( "669E2765-BE03-4616-A471-B87B86807AB7" );

            // Attrib for BlockType: com .ccvonline - Residency Competency Person Project Assignment Assessment List:Detail Page
            DeleteAttribute( "710536D1-2664-4F48-ABBF-4DAD54A2DABB" );

            // Attrib for BlockType: Group List:Show IsSystem
            DeleteAttribute( "7CDBC199-D661-4FF8-B1FD-1F7F822F69BE" );

            // Attrib for BlockType: Group List:Show GroupType
            DeleteAttribute( "242DF3C9-4BE8-4216-9AAE-CA19B9B8FC91" );

            // Attrib for BlockType: com .ccvonline - Residency Competency Person Project Detail:Residency Competency Person Page
            DeleteAttribute( "A718D831-2126-415D-A147-E67B78F8419F" );

            // Attrib for BlockType: com .ccvonline - Residency Competency Person Project List:Detail Page
            DeleteAttribute( "01FD5A4D-3A9C-4F98-8955-E131C3500B23" );

            // Attrib for BlockType: com .ccvonline - Residency Competency Person Project Assignment List:Detail Page
            DeleteAttribute( "3176A7C9-831F-403B-A549-AF1E1B4BC3D9" );

            // Attrib for BlockType: com .ccvonline - Residency Competency Person List:Detail Page
            DeleteAttribute( "6E623135-F71C-481A-9A75-CD77EE0D6D81" );

            // Attrib for BlockType: com .ccvonline - Residency Person List:Detail Page
            DeleteAttribute( "7B1CA61D-0E1A-44C9-9210-DF49D29EECF0" );

            DeleteBlock( "4F104290-3664-40A7-9F6F-F0083900C9C6" ); // Group Member Detail
            DeleteBlock( "72768282-BDCE-49AD-B325-F272EEC9B1F4" ); // Residency Competency Person Project Assignment Assessment Point Of Assessment Detail
            DeleteBlock( "F32573AA-09E0-41C7-B614-A0AB88D8BDCD" ); // Residency Competency Person Project Assignment Assessment Detail
            DeleteBlock( "E32DA215-9919-4245-9799-0328AAA7E564" ); // Residency Competency Person Project Assignment Assessment Point Of Assessment List
            DeleteBlock( "8B03F77F-4EAB-410B-9761-9CFE09CC0D3F" ); // Residency Competency Person Project Assignment Assessment List
            DeleteBlock( "BF51E84E-F37A-48CC-AAFB-D5A4FDEFF79E" ); // Residency Competency Person Project Assignment Detail
            DeleteBlock( "31FFF300-F890-4243-B2A1-437F69ACE986" ); // Residency Competency Person Project Assignment List
            DeleteBlock( "F377C5DC-0618-4259-AA32-B75959CBEC85" ); // Residency Competency Person Project Detail
            DeleteBlock( "2A39AD5B-3497-4B47-9A66-1CEA68FD848C" ); // Residency Competency Person Project List
            DeleteBlock( "AD59F37C-97EC-4F07-A604-3AAF8270C737" ); // Group List
            DeleteBlock( "746B438D-75E3-495A-9678-C3C14629511A" ); // Group Detail
            DeleteBlock( "71D6DCB8-6CEC-4703-8088-9B3FC0DEE7A6" ); // Residency Competency Person Detail
            DeleteBlock( "3F70C055-32CF-4F18-BBBB-6A0C88C5BE39" ); // Residency Competency Person List
            DeleteBlock( "9CEC61A6-BEA6-420B-AEE1-8424E33998CB" ); // Residency Person Detail
            DeleteBlock( "A66E242C-D1E0-4F4E-9F6A-ED7484352BFE" ); // Residency Person List
            DeleteBlockType( "A102FA19-85A6-4701-9C14-29F1A412FDC0" ); // com .ccvonline - Residency Competency Person Project Assignment Assessment Point Of Assessment De...
            DeleteBlockType( "0E6DAF91-AE77-4196-91B5-D1E3CB3B2403" ); // com .ccvonline - Residency Competency Person Project Assignment Assessment POA List
            DeleteBlockType( "6285726B-A1C3-407C-A1C6-B4D18943A799" ); // com .ccvonline - Residency Competency Person Project Assignment Assessment POA Detail
            DeleteBlockType( "9EEA2EF7-E49D-4CAB-B067-BE4ECB9FF376" ); // com .ccvonline - Residency Competency Person Project Assignment Assessment Detail
            DeleteBlockType( "5847D528-98BB-487C-BE26-A8FF60F74033" ); // com .ccvonline - Residency Competency Person Project Detail
            DeleteBlockType( "C947DC2C-76E1-4B69-8FC6-E9E3134B36C7" ); // com .ccvonline - Residency Competency Person Project List
            DeleteBlockType( "9C373F55-7E44-4641-AA6B-A30E7F214F37" ); // com .ccvonline - Residency Competency Person Project Assignment List
            DeleteBlockType( "8A5FB3E3-4147-4DE0-9CAE-20974ADD5E70" ); // com .ccvonline - Residency Competency Person Project Assignment Detail
            DeleteBlockType( "203C01AB-0EE6-4EE0-A934-EB0FAF426E9C" ); // com .ccvonline - Residency Competency Person Project Assignment Assessment Point Of Assessment List
            DeleteBlockType( "0B86F65A-904B-4FEE-86BF-99E1C1A696F5" ); // com .ccvonline - Residency Competency Person Project Assignment Assessment List
            DeleteBlockType( "E4A531AD-4FCF-449B-91AB-ACBF87D83881" ); // com .ccvonline - Residency Competency Person List
            DeleteBlockType( "9E3D6BF6-28EE-4902-B6CD-AC1C31A7B731" ); // com .ccvonline - Residency Competency Person Detail
            DeleteBlockType( "F0A0BE3A-DD15-468F-93A6-97C440DB8253" ); // com .ccvonline - Residency Person Detail
            DeleteBlockType( "13EE0E6A-BBC6-4D86-9226-E246CFBA11B2" ); // com .ccvonline - Residency Person List
            DeleteBlockType( "1A68D507-5480-4EA9-94F8-D81E806506FE" ); // CRM - Person Detail - Add Family
            DeleteBlockType( "57FCB340-BD88-4A06-AF1E-E2ECF81E9909" ); // Administration - Attribute Categories
            DeleteBlockType( "782DF88E-1C37-430E-8EF0-F7A7EAE09F3F" ); // Utility - Linq Grid
            DeleteBlockType( "2C071825-8ABF-490A-9CCC-9272128E8C6A" ); // Finance - Giving Profile List
            DeleteBlockType( "D9CF9085-60C1-4E0A-878D-481A6B333CF1" ); // Finance - Giving Profile Detail
            DeleteBlockType( "A9CE842F-C771-4C66-B780-384CB652B092" ); // CRM - Person Detail - Notes
            DeleteBlockType( "FFBC2404-BC6C-41F5-918F-875311536E2D" ); // Check In - Mobile Entry
            DeleteBlockType( "160D9A11-3B24-4345-9234-0627270F79CB" ); // Check In - Ability Level Select
            DeletePage( "69A714EB-1870-4516-AB4F-63ADF2100FEA" ); // Project Assignment Point of Assessment
            DeletePage( "32E7BCDE-37BC-48B5-B0FE-AA784AF0425A" ); // Resident Project Assignment Assessment Detail
            DeletePage( "165E7CB7-E15A-4AC2-8383-567A593279F0" ); // Project Assignment Detail
            DeletePage( "39661338-971E-45EA-86C3-7A8A5D2DEA54" ); // Resident Project Detail
            DeletePage( "6F095271-8060-4577-8E72-C0EE2389527C" ); // Resident Competency Detail
            DeletePage( "57F487E8-46E4-4F6E-B03E-96CC2C4BE185" ); // Resident Detail
            DeletePage( "531C64E2-282B-4644-9619-319BDBAC627E" ); // Residents
            DeletePage( "36428AF8-7650-4047-B655-8D39F5EA10C5" ); // Resident Groups
        }
    }
}
