using System;
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
    public class Project : NamedModel<Project>
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
        public int CompetencyId { get; set; }

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
        public virtual Competency Competency { get; set; }

        /// <summary>
        /// Gets or sets the residency project point of assessments.
        /// </summary>
        /// <value>
        /// The residency project point of assessments.
        /// </value>
        public virtual List<ProjectPointOfAssessment> ProjectPointOfAssessments { get; set; }

        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public class ProjectConfiguration : EntityTypeConfiguration<Project>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectConfiguration"/> class.
        /// </summary>
        public ProjectConfiguration()
        {
            this.HasRequired( p => p.Competency ).WithMany(a => a.Projects).HasForeignKey( p => p.CompetencyId ).WillCascadeOnDelete( false );
        }
    }
}
