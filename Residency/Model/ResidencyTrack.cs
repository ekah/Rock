using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using com.ccvonline.Residency.Data;

namespace com.ccvonline.Residency.Model
{
    /// <summary>
    /// 
    /// </summary>
    [Table( "_com_ccvonline_ResidencyTrack" )]
    [DataContract]
    public class ResidencyTrack : ResidencyNamedModel<ResidencyTrack>
    {
        #region Entity Properties

        /// <summary>
        /// Gets or sets the residency period id.
        /// </summary>
        /// <value>
        /// The residency period id.
        /// </value>
        [Required]
        [DataMember(Name="_com_ccvonline_ResidencyPeriodId", IsRequired=true)]
        public int ResidencyPeriodId { get; set; }

        #endregion

        #region Virtual Properties

        /// <summary>
        /// Gets or sets the residency period.
        /// </summary>
        /// <value>
        /// The residency period.
        /// </value>
        public virtual ResidencyPeriod ResidencyPeriod { get; set; }

        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public partial class ResidencyTrackConfiguration : EntityTypeConfiguration<ResidencyTrack>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResidencyTrackConfiguration"/> class.
        /// </summary>
        public ResidencyTrackConfiguration()
        {
            this.HasRequired( p => p.ResidencyPeriod ).WithMany( p => p.ResidencyTracks ).HasForeignKey( p => p.ResidencyPeriodId ).WillCascadeOnDelete( false );
        }
    }
}
