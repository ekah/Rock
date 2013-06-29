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
    public class CompetencyPersonProjectAssignmentAssessment : Model<CompetencyPersonProjectAssignmentAssessment>
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
        public int CompetencyPersonProjectAssignmentId { get; set; }

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
        public decimal? OverallRating { get; set; }

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
        public virtual CompetencyPersonProjectAssignment CompetencyPersonProjectAssignment { get; set; }

        /// <summary>
        /// Gets or sets the competency person project assignment assessment point of assessments.
        /// </summary>
        /// <value>
        /// The competency person project assignment assessment point of assessments.
        /// </value>
        public virtual List<CompetencyPersonProjectAssignmentAssessmentPointOfAssessment> CompetencyPersonProjectAssignmentAssessmentPointOfAssessments { get; set; }

        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public partial class CompetencyPersonProjectAssignmentAssessmentConfiguration : EntityTypeConfiguration<CompetencyPersonProjectAssignmentAssessment>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CompetencyPersonProjectAssignmentAssessmentConfiguration"/> class.
        /// </summary>
        public CompetencyPersonProjectAssignmentAssessmentConfiguration()
        {
            this.HasRequired( a => a.CompetencyPersonProjectAssignment ).WithMany( a => a.CompetencyPersonProjectAssignmentAssessments ).HasForeignKey( a => a.CompetencyPersonProjectAssignmentId ).WillCascadeOnDelete( false );
            
            // limit OverallRating to one decimal point
            this.Property( m => m.OverallRating ).HasPrecision( 2, 1 );
        }
    }
}
