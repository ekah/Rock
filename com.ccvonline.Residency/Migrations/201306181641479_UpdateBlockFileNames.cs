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
    public partial class UpdateBlockFileNames : Rock.Migrations.RockMigration
    {
        /// <summary>
        /// Operations to be performed during the upgrade process.
        /// </summary>
        public override void Up()
        {
            Sql( @"
update [dbo].[BlockType] set [Path] = replace([Path],'~/Plugins/com.ccvonline/Residency','~/Plugins/com.ccvonline/Residency/') where [Guid] in (
'81c5ee50-ae8d-45f9-8014-a7c65f0fdbd6',
'511421db-e127-447d-81a6-ff8c52d11815',
'a3e2f4b9-fc87-472a-b873-2bb649c2417b',
'72133176-4e1a-4851-b4d0-bbc447d84440',
'488bb996-8f4b-4db4-9b0b-fb7b959bcdaf',
'd1d1c418-b84b-4307-b4ec-d2fd2970d639',
'1a1e32b5-93bc-480e-bfb4-a5d9c06dcbf2',
'8ba15032-d16a-4fdc-ae7f-a77f50267f39',
'8eee930e-f879-48dc-8afb-7249b618034d',
'a56e3be8-ab33-4cea-9c93-f138b7e24498',
'13ee0e6a-bbc6-4d86-9226-e246cfba11b2',
'f0a0be3a-dd15-468f-93a6-97c440db8253',
'9e3d6bf6-28ee-4902-b6cd-ac1c31a7b731',
'e4a531ad-4fcf-449b-91ab-acbf87d83881',
'0b86f65a-904b-4fee-86bf-99e1c1a696f5',
'203c01ab-0ee6-4ee0-a934-eb0faf426e9c',
'8a5fb3e3-4147-4de0-9cae-20974add5e70',
'9c373f55-7e44-4641-aa6b-a30e7f214f37',
'c947dc2c-76e1-4b69-8fc6-e9e3134b36c7',
'5847d528-98bb-487c-be26-a8ff60f74033',
'9eea2ef7-e49d-4cab-b067-be4ecb9ff376',
'6285726b-a1c3-407c-a1c6-b4d18943a799',
'0e6daf91-ae77-4196-91b5-d1e3cb3b2403',
'a102fa19-85a6-4701-9c14-29f1a412fdc0')
and [Path] not like '~/Plugins/com.ccvonline/Residency/%'" );

        }
        
        /// <summary>
        /// Operations to be performed during the downgrade process.
        /// </summary>
        public override void Down()
        {
            // don't undo path rename
        }
    }
}
