﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

using com.ccvonline.Data;
using com.ccvonline.Residency.Data;

namespace com.ccvonline.Residency.Model
{
    /// <summary>
    /// 
    /// </summary>
    [Table( "_com_ccvonline_Residency_Track" )]
    [DataContract]
    public class Track : NamedModel<Track>
    {
        #region Entity Properties

        /// <summary>
        /// Gets or sets the residency period id.
        /// </summary>
        /// <value>
        /// The residency period id.
        /// </value>
        [Required]
        [DataMember]
        public int PeriodId { get; set; }

        /// <summary>
        /// Gets or sets the display order.
        /// </summary>
        /// <value>
        /// The display order.
        /// </value>
        [Required]
        [DataMember]
        public int DisplayOrder { get; set; }

        #endregion

        #region Virtual Properties

        /// <summary>
        /// Gets or sets the residency period.
        /// </summary>
        /// <value>
        /// The residency period.
        /// </value>
        public virtual Period Period { get; set; }

        /// <summary>
        /// Gets or sets the residency competencies.
        /// </summary>
        /// <value>
        /// The residency competencies.
        /// </value>
        public virtual List<Competency> Competencies { get; set; }

        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public partial class TrackConfiguration : EntityTypeConfiguration<Track>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TrackConfiguration"/> class.
        /// </summary>
        public TrackConfiguration()
        {
            this.HasRequired( p => p.Period ).WithMany( p => p.Tracks ).HasForeignKey( p => p.PeriodId ).WillCascadeOnDelete( false );
        }
    }
}
