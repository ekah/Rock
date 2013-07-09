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
    public partial class GradeEmailTemplate : Rock.Migrations.RockMigration
    {
        /// <summary>
        /// Operations to be performed during the upgrade process.
        /// </summary>
        public override void Up()
        {
            Sql( @"
DELETE FROM [EmailTemplate] WHERE [Guid] = 'CCEDEC52-EC8A-41BF-9F78-C60418835257'

INSERT INTO [EmailTemplate] ([IsSystem], [PersonId], [Category], [Title], [From], [To], [Cc], [Bcc], [Subject], [Body], [Guid]) 
VALUES (1, NULL, 'Residency', 'Project Grade Request', 'rock@sparkdevnetwork.com', '', '', '', 'Project Grade Request', 
'{{ EmailHeader }}

{{ Facilitator.FirstName }},<br/><br/>

{{Resident.FullName}} requests that you <a href=''{{ GradeDetailPageUrl }}''>grade</a> {{ Project.Name }} - {{ Project.Description}} 
<br/>
<br/>
Thank-you,<br/>
{{ OrganizationName }}  

{{ EmailFooter }}', 'CCEDEC52-EC8A-41BF-9F78-C60418835257')
" );
        }
        
        /// <summary>
        /// Operations to be performed during the downgrade process.
        /// </summary>
        public override void Down()
        {
            Sql( @"DELETE FROM [EmailTemplate] WHERE [Guid] = 'CCEDEC52-EC8A-41BF-9F78-C60418835257'" );
        }
    }
}
