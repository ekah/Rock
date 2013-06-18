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
    [Table( "_com_ccvonline_ResidencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessment" )]
    [DataContract]
    public class ResidencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessment : Model<ResidencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessment>
    {
        #region Entity Properties
        
        /// <summary>
        /// Gets or sets the residency competency person project assignment assessment id.
        /// </summary>
        /// <value>
        /// The residency competency person project assignment assessment id.
        /// </value>
        [Required]
        [DataMember]
        public int ResidencyCompetencyPersonProjectAssignmentAssessmentId { get; set; }
        
        /// <summary>
        /// Gets or sets the residency project point of assessment id.
        /// </summary>
        /// <value>
        /// The residency project point of assessment id.
        /// </value>
        [Required]
        [DataMember]
        public int ResidencyProjectPointOfAssessmentId { get; set; }
        
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
        
        #endregion

        #region Virtual Properties

        /// <summary>
        /// Gets or sets the residency competency person project assignment assessment.
        /// </summary>
        /// <value>
        /// The residency competency person project assignment assessment.
        /// </value>
        public virtual ResidencyCompetencyPersonProjectAssignmentAssessment ResidencyCompetencyPersonProjectAssignmentAssessment { get; set; }

        /// <summary>
        /// Gets or sets the residency project point of assessment.
        /// </summary>
        /// <value>
        /// The residency project point of assessment.
        /// </value>
        public virtual ResidencyProjectPointOfAssessment ResidencyProjectPointOfAssessment { get; set; }

        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public partial class ResidencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessmentConfiguration : EntityTypeConfiguration<ResidencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessment>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResidencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessmentConfiguration"/> class.
        /// </summary>
        public ResidencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessmentConfiguration()
        {
            this.HasRequired( a => a.ResidencyCompetencyPersonProjectAssignmentAssessment ).WithMany().HasForeignKey( a => a.ResidencyCompetencyPersonProjectAssignmentAssessmentId ).WillCascadeOnDelete( false );
            this.HasRequired( a => a.ResidencyProjectPointOfAssessment ).WithMany().HasForeignKey( a => a.ResidencyProjectPointOfAssessmentId ).WillCascadeOnDelete( false );
        }
    }
}
