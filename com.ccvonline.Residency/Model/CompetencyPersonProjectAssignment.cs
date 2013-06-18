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
    [Table( "_com_ccvonline_Residency_CompetencyPersonProjectAssignment" )]
    [DataContract]
    public class CompetencyPersonProjectAssignment : Model<CompetencyPersonProjectAssignment>
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
        public int CompetencyPersonProjectId { get; set; }

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
        public virtual CompetencyPersonProject CompetencyPersonProject { get; set; }

        /// <summary>
        /// Gets or sets the assessor person.
        /// </summary>
        /// <value>
        /// The assessor person.
        /// </value>
        public virtual Rock.Model.Person AssessorPerson { get; set; }

        /// <summary>
        /// Gets or sets the residency competency person project assignment assessments.
        /// </summary>
        /// <value>
        /// The residency competency person project assignment assessments.
        /// </value>
        public virtual List<CompetencyPersonProjectAssignmentAssessment> CompetencyPersonProjectAssignmentAssessments { get; set; }

        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public partial class CompetencyPersonProjectAssignmentConfiguration : EntityTypeConfiguration<CompetencyPersonProjectAssignment>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CompetencyPersonProjectAssignmentConfiguration"/> class.
        /// </summary>
        public CompetencyPersonProjectAssignmentConfiguration()
        {
            this.HasRequired( a => a.CompetencyPersonProject ).WithMany(a => a.CompetencyPersonProjectAssignments).HasForeignKey( a => a.CompetencyPersonProjectId ).WillCascadeOnDelete( false );
            this.HasOptional( a => a.AssessorPerson ).WithMany().HasForeignKey( a => a.AssessorPersonId ).WillCascadeOnDelete( false );
        }
    }
}
