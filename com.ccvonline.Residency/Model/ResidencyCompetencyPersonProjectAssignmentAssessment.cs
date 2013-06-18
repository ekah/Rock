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
    [Table( "_com_ccvonline_Residency_CompetencyPersonProjectAssignmentAssessment" )]
    [DataContract]
    public class ResidencyCompetencyPersonProjectAssignmentAssessment : Model<ResidencyCompetencyPersonProjectAssignmentAssessment>
    {
        #region Entity Properties

        /// <summary>
        /// Gets or sets the residency competency person project assignment id.
        /// </summary>
        /// <value>
        /// The residency competency person project assignment id.
        /// </value>
        [DataMember]
        [Required]
        public int ResidencyCompetencyPersonProjectAssignmentId { get; set; }

        /// <summary>
        /// Gets or sets the assessment date time.
        /// </summary>
        /// <value>
        /// The assessment date time.
        /// </value>
        [DataMember]
        public DateTime? AssessmentDateTime { get; set; }

        /// <summary>
        /// Gets or sets the rating.
        /// </summary>
        /// <value>
        /// The rating.
        /// </value>
        [DataMember]
        public int? Rating { get; set; }

        /// <summary>
        /// Gets or sets the rating notes.
        /// </summary>
        /// <value>
        /// The rating notes.
        /// </value>
        [DataMember]
        public string RatingNotes { get; set; }

        /// <summary>
        /// Gets or sets the resident comments.
        /// </summary>
        /// <value>
        /// The resident comments.
        /// </value>
        [DataMember]
        public string ResidentComments { get; set; }
        
        #endregion

        #region Virtual Properties

        /// <summary>
        /// Gets or sets the residency competency person project assignment.
        /// </summary>
        /// <value>
        /// The residency competency person project assignment.
        /// </value>
        public virtual ResidencyCompetencyPersonProjectAssignment ResidencyCompetencyPersonProjectAssignment { get; set; }

        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public partial class ResidencyCompetencyPersonProjectAssignmentAssessmentConfiguration : EntityTypeConfiguration<ResidencyCompetencyPersonProjectAssignmentAssessment>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResidencyCompetencyPersonProjectAssignmentAssessmentConfiguration"/> class.
        /// </summary>
        public ResidencyCompetencyPersonProjectAssignmentAssessmentConfiguration()
        {
            this.HasRequired( a => a.ResidencyCompetencyPersonProjectAssignment ).WithMany( a => a.ResidencyCompetencyPersonProjectAssignmentAssessments ).HasForeignKey( a => a.ResidencyCompetencyPersonProjectAssignmentId ).WillCascadeOnDelete( false );
        }
    }
}
