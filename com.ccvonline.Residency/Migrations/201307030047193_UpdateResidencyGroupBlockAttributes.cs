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
    public partial class UpdateResidencyGroupBlockAttributes : Rock.Migrations.RockMigration
    {
        /// <summary>
        /// Operations to be performed during the upgrade process.
        /// </summary>
        public override void Up()
        {
            // Residency Groups - Group List: Limit to GroupType Residency
            AddBlockAttributeValue( "AD59F37C-97EC-4F07-A604-3AAF8270C737", "C3FD6CE3-D37F-4A53-B0D7-AB1B1F252324", "00043ce6-eb1b-43b5-a12a-4552b91a3e28" );

            // Residency Groups - Group Detail: Limit to GroupType Residency
            AddBlockAttributeValue( "746B438D-75E3-495A-9678-C3C14629511A", "15AC7A62-7BF2-44B7-93CD-EA8F96BF529A", "00043ce6-eb1b-43b5-a12a-4552b91a3e28" );

        }

        /// <summary>
        /// Operations to be performed during the downgrade process.
        /// </summary>
        public override void Down()
        {
        }
    }
}
