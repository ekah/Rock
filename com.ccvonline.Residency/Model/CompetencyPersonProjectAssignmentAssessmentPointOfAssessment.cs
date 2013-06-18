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
    [Table( "_com_ccvonline_Residency_CompetencyPersonProjectAssignmentAssessmentPointOfAssessment" )]
    [DataContract]
    public class CompetencyPersonProjectAssignmentAssessmentPointOfAssessment : Model<CompetencyPersonProjectAssignmentAssessmentPointOfAssessment>
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
        public int CompetencyPersonProjectAssignmentAssessmentId { get; set; }
        
        /// <summary>
        /// Gets or sets the residency project point of assessment id.
        /// </summary>
        /// <value>
        /// The residency project point of assessment id.
        /// </value>
        [Required]
        [DataMember]
        public int ProjectPointOfAssessmentId { get; set; }
        
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
        public virtual CompetencyPersonProjectAssignmentAssessment CompetencyPersonProjectAssignmentAssessment { get; set; }

        /// <summary>
        /// Gets or sets the residency project point of assessment.
        /// </summary>
        /// <value>
        /// The residency project point of assessment.
        /// </value>
        public virtual ProjectPointOfAssessment ProjectPointOfAssessment { get; set; }

        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public partial class CompetencyPersonProjectAssignmentAssessmentPointOfAssessmentConfiguration : EntityTypeConfiguration<CompetencyPersonProjectAssignmentAssessmentPointOfAssessment>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CompetencyPersonProjectAssignmentAssessmentPointOfAssessmentConfiguration"/> class.
        /// </summary>
        public CompetencyPersonProjectAssignmentAssessmentPointOfAssessmentConfiguration()
        {
            this.HasRequired( a => a.CompetencyPersonProjectAssignmentAssessment ).WithMany().HasForeignKey( a => a.CompetencyPersonProjectAssignmentAssessmentId ).WillCascadeOnDelete( false );
            this.HasRequired( a => a.ProjectPointOfAssessment ).WithMany().HasForeignKey( a => a.ProjectPointOfAssessmentId ).WillCascadeOnDelete( false );
        }
    }
}
