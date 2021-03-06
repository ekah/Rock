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
           ,[ShowInGroupList]
           ,[IconCssClass]
           ,[Guid])
     VALUES
           (0
           ,'Residency'
           ,'Group Types for the Residency program'
           ,'Resident'
           ,null
           ,1
           ,1
           ,'icon-md'
           ,'00043CE6-EB1B-43B5-A12A-4552B91A3E28')

select @groupTypeId = @@IDENTITY

INSERT INTO [dbo].[GroupRole] 
    ([IsSystem] ,[GroupTypeId] ,[Name] ,[Description] ,[SortOrder] ,[MaxCount] ,[MinCount] ,[Guid] ,[IsLeader])
     VALUES
    (0, @groupTypeId, 'Resident', 'A Resident in the Residency program', 0, null, null, 'AC1CD9C9-782C-42A6-A28B-78B38C3AC833', 0)

update [GroupType] set [DefaultGroupRoleId] = @@IDENTITY where [Guid] = '00043CE6-EB1B-43B5-A12A-4552B91A3E28'

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
