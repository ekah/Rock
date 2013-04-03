using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.ccvonline.Residency.Model;

namespace com.ccvonline.Residency.Data
{
    /// <summary>
    /// 
    /// </summary>
    public partial class ResidencyContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResidencyContext"/> class.
        /// </summary>
        public ResidencyContext()
            : base( "RockContext" )
        {
        }
            
        
        #region Models

        /// <summary>
        /// Gets or sets the residency periods.
        /// </summary>
        /// <value>
        /// The residency periods.
        /// </value>
        public DbSet<ResidencyPeriod> ResidencyPeriods { get; set; }

        /// <summary>
        /// Gets or sets the residency tracks.
        /// </summary>
        /// <value>
        /// The residency tracks.
        /// </value>
        public DbSet<ResidencyTrack> ResidencyTracks { get; set; }
        
        #endregion
    }

    public static class ContextHelper
    {
        /// <summary>
        /// Adds the configurations.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        public static void AddConfigurations( DbModelBuilder modelBuilder )
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();


            modelBuilder.Configurations.Add( new ResidencyPeriodConfiguration() );
            modelBuilder.Configurations.Add( new ResidencyTrackConfiguration() );
        }
    }
}
