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
    [Table("_com_ccvonline_Residency_Project")]
    [DataContract]
    public class ResidencyProject : NamedModel<ResidencyProject>
    {
        #region Entity Properties

        /// <summary>
        /// Gets or sets the residency competency id.
        /// </summary>
        /// <value>
        /// The residency competency id.
        /// </value>
        [Required]
        [DataMember]
        public int ResidencyCompetencyId { get; set; }

        /// <summary>
        /// Gets or sets the min assignment count default.
        /// </summary>
        /// <value>
        /// The min assignment count default.
        /// </value>
        [DataMember]
        public int? MinAssignmentCountDefault { get; set; }

        #endregion

        #region Virtual Properties

        /// <summary>
        /// Gets or sets the residency competency.
        /// </summary>
        /// <value>
        /// The residency competency.
        /// </value>
        public virtual ResidencyCompetency ResidencyCompetency { get; set; }

        /// <summary>
        /// Gets or sets the residency project point of assessments.
        /// </summary>
        /// <value>
        /// The residency project point of assessments.
        /// </value>
        public virtual List<ResidencyProjectPointOfAssessment> ResidencyProjectPointOfAssessments { get; set; }

        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public class ResidencyProjectConfiguration : EntityTypeConfiguration<ResidencyProject>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResidencyProjectConfiguration"/> class.
        /// </summary>
        public ResidencyProjectConfiguration()
        {
            this.HasRequired( p => p.ResidencyCompetency ).WithMany(a => a.ResidencyProjects).HasForeignKey( p => p.ResidencyCompetencyId ).WillCascadeOnDelete( false );
        }
    }
}
