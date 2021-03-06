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
    [Table( "_com_ccvonline_Residency_CompetencyPersonProjectAssessmentPointOfAssessment" )]
    [DataContract]
    public class CompetencyPersonProjectAssessmentPointOfAssessment : com.ccvonline.Data.Model<CompetencyPersonProjectAssessmentPointOfAssessment>
    {
        #region Entity Properties

        /// <summary>
        /// Gets or sets the competency person project assessment id.
        /// </summary>
        /// <value>
        /// The competency person project assessment id.
        /// </value>
        [Required]
        [DataMember]
        public int CompetencyPersonProjectAssessmentId { get; set; }

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
        /// Gets or sets the competency person project assessment.
        /// </summary>
        /// <value>
        /// The competency person project assessment.
        /// </value>
        public virtual CompetencyPersonProjectAssessment CompetencyPersonProjectAssessment { get; set; }

        /// <summary>
        /// Gets or sets the residency project point of assessment.
        /// </summary>
        /// <value>
        /// The residency project point of assessment.
        /// </value>
        public virtual ProjectPointOfAssessment ProjectPointOfAssessment { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format( "{0}, Point of Assessment: {1}", CompetencyPersonProjectAssessment, ProjectPointOfAssessment );
        }

        #endregion

    }

    /// <summary>
    /// 
    /// </summary>
    public partial class CompetencyPersonProjectAssessmentPointOfAssessmentConfiguration : EntityTypeConfiguration<CompetencyPersonProjectAssessmentPointOfAssessment>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CompetencyPersonProjectAssessmentPointOfAssessmentConfiguration"/> class.
        /// </summary>
        public CompetencyPersonProjectAssessmentPointOfAssessmentConfiguration()
        {
            this.HasRequired( a => a.CompetencyPersonProjectAssessment ).WithMany( x => x.CompetencyPersonProjectAssessmentPointOfAssessments ).HasForeignKey( a => a.CompetencyPersonProjectAssessmentId ).WillCascadeOnDelete( false );
            this.HasRequired( a => a.ProjectPointOfAssessment ).WithMany().HasForeignKey( a => a.ProjectPointOfAssessmentId ).WillCascadeOnDelete( false );
        }
    }
}
