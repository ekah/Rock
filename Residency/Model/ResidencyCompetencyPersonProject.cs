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
    [Table( "_com_ccvonline_ResidencyCompetencyPersonProject" )]
    [DataContract]
    public class ResidencyCompetencyPersonProject : ResidencyModel<ResidencyCompetencyPersonProject>
    {
        #region Entity Properties

        /// <summary>
        /// Gets or sets the residency competency person id.
        /// </summary>
        /// <value>
        /// The residency competency person id.
        /// </value>
        [Required]
        [DataMember]
        public int ResidencyCompetencyPersonId { get; set; }

        /// <summary>
        /// Gets or sets the residency project id.
        /// </summary>
        /// <value>
        /// The residency project id.
        /// </value>
        [Required]
        [DataMember]
        public int ResidencyProjectId { get; set; }


        /// <summary>
        /// Gets or sets the min assignment count.
        /// </summary>
        /// <value>
        /// The min assignment count.
        /// </value>
        [DataMember]
        public int? MinAssignmentCount { get; set; }
        
        #endregion

        #region Virtual Properties

        /// <summary>
        /// Gets or sets the residency competency person.
        /// </summary>
        /// <value>
        /// The residency competency person.
        /// </value>
        public virtual ResidencyCompetencyPerson ResidencyCompetencyPerson { get; set; }

        /// <summary>
        /// Gets or sets the residency project.
        /// </summary>
        /// <value>
        /// The residency project.
        /// </value>
        public virtual ResidencyProject ResidencyProject { get; set; }

        /// <summary>
        /// Gets or sets the residency competency person project assignments.
        /// </summary>
        /// <value>
        /// The residency competency person project assignments.
        /// </value>
        public virtual List<ResidencyCompetencyPersonProjectAssignment> ResidencyCompetencyPersonProjectAssignments { get; set; }

        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public partial class ResidencyCompetencyPersonProjectConfiguration : EntityTypeConfiguration<ResidencyCompetencyPersonProject>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResidencyCompetencyPersonProjectConfiguration"/> class.
        /// </summary>
        public ResidencyCompetencyPersonProjectConfiguration()
        {
            this.HasRequired( a => a.ResidencyCompetencyPerson ).WithMany().HasForeignKey( a => a.ResidencyCompetencyPersonId ).WillCascadeOnDelete( false );
            this.HasRequired( a => a.ResidencyProject ).WithMany().HasForeignKey( a => a.ResidencyProjectId ).WillCascadeOnDelete( false );
        }
    }
}
