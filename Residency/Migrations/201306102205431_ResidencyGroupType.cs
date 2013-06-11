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
    public partial class ResidencyGroupType : Rock.Migrations.RockMigration
    {
        /// <summary>
        /// Operations to be performed during the upgrade process.
        /// </summary>
        public override void Up()
        {
            Sql( @"
begin

declare 
  @groupTypeId int

INSERT INTO [dbo].[GroupType]
           ([IsSystem]
           ,[Name]
           ,[Description]
           ,[GroupMemberTerm]
           ,[DefaultGroupRoleId]
           ,[ShowInNavigation]
           ,[IconCssClass]
           ,[Guid])
     VALUES
           (0
           ,'Residency'
           ,'Group Types for the Residency program'
           ,'Resident'
           ,null
           ,1
           ,'icon-md'
           ,'00043CE6-EB1B-43B5-A12A-4552B91A3E28')

select @groupTypeId = @@IDENTITY

INSERT INTO [dbo].[GroupRole] 
    ([IsSystem] ,[GroupTypeId] ,[Name] ,[Description] ,[SortOrder] ,[MaxCount] ,[MinCount] ,[Guid] ,[IsLeader])
     VALUES
    (0, @groupTypeId, 'Resident', 'A Resident in the Residency program', 0, null, null, 'AC1CD9C9-782C-42A6-A28B-78B38C3AC833', 0)

update [GroupType] set [DefaultGroupRoleId] = @@IDENTITY where [Guid] = '00043CE6-EB1B-43B5-A12A-4552B91A3E28'



--select newid()
INSERT INTO [dbo].[Group] ([IsSystem],[ParentGroupId],[GroupTypeId],[CampusId],[Name],[Description],[IsSecurityRole],[IsActive],[Guid])
                            VALUES (0,null,@groupTypeId,null,'Residents','Residents in the Residency program',0,1,'4B7D22E8-B08C-42DC-B1F1-F2834BC8D1DF');


end
" );
        }
        
        /// <summary>
        /// Operations to be performed during the downgrade process.
        /// </summary>
        public override void Down()
        {
            Sql( @"
update [GroupType] set [DefaultGroupRoleId] = null where [Guid] = '00043CE6-EB1B-43B5-A12A-4552B91A3E28';
delete from [Group] where [GroupTypeId] in (select Id from [GroupType] where [Guid] = '00043CE6-EB1B-43B5-A12A-4552B91A3E28');
delete from [GroupRole] where [GroupTypeId] in (select Id from [GroupType] where [Guid] = '00043CE6-EB1B-43B5-A12A-4552B91A3E28');
delete from [GroupType] where [Guid] = '00043CE6-EB1B-43B5-A12A-4552B91A3E28';
" );
        }
    }
}
/* Skipped Operations for tables that are not part of ResidencyContext: Review these comments to verify the proper things were skipped */
/* To disable skipping, edit your Migrations\Configuration.cs so that CodeGenerator = new RockCSharpMigrationCodeGenerator<ResidencyContext>(false); */

// Up()...
// AddColumnOperation for TableName GroupType, column ShowInNavigation.

// Down()...
// DropColumnOperation for TableName GroupType, column ShowInNavigation.
