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
    [Table( "_com_ccvonline_ResidencyCompetencyPersonProjectAssignment" )]
    [DataContract]
    public class ResidencyCompetencyPersonProjectAssignment : ResidencyModel<ResidencyCompetencyPersonProjectAssignment>
    {
        #region Entity Properties

        /// <summary>
        /// Gets or sets the residency competency person project id.
        /// </summary>
        /// <value>
        /// The residency competency person project id.
        /// </value>
        [Required]
        [DataMember]
        public int ResidencyCompetencyPersonProjectId { get; set; }

        /// <summary>
        /// Gets or sets the assessor person id.
        /// </summary>
        /// <value>
        /// The assessor person id.
        /// </value>
        [DataMember]
        public int? AssessorPersonId { get; set; }

        /// <summary>
        /// Gets or sets the completed date time.
        /// </summary>
        /// <value>
        /// The completed date time.
        /// </value>
        [DataMember]
        public DateTime? CompletedDateTime { get; set; }

        #endregion

        #region Virtual Properties

        /// <summary>
        /// Gets or sets the residency competency person project.
        /// </summary>
        /// <value>
        /// The residency competency person project.
        /// </value>
        public ResidencyCompetencyPersonProject ResidencyCompetencyPersonProject { get; set; }

        /// <summary>
        /// Gets or sets the assessor person.
        /// </summary>
        /// <value>
        /// The assessor person.
        /// </value>
        public Rock.Model.Person AssessorPerson { get; set; }

        /// <summary>
        /// Gets or sets the residency competency person project assignment assessments.
        /// </summary>
        /// <value>
        /// The residency competency person project assignment assessments.
        /// </value>
        public List<ResidencyCompetencyPersonProjectAssignmentAssessment> ResidencyCompetencyPersonProjectAssignmentAssessments { get; set; }

        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public partial class ResidencyCompetencyPersonProjectAssignmentConfiguration : EntityTypeConfiguration<ResidencyCompetencyPersonProjectAssignment>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResidencyCompetencyPersonProjectAssignmentConfiguration"/> class.
        /// </summary>
        public ResidencyCompetencyPersonProjectAssignmentConfiguration()
        {
            this.HasRequired( a => a.ResidencyCompetencyPersonProject ).WithMany(a => a.ResidencyCompetencyPersonProjectAssignments).HasForeignKey( a => a.ResidencyCompetencyPersonProjectId ).WillCascadeOnDelete( false );
            this.HasOptional( a => a.AssessorPerson ).WithMany().HasForeignKey( a => a.AssessorPersonId ).WillCascadeOnDelete( false );
        }
    }
}
