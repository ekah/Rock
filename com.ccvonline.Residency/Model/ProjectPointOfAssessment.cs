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
using Rock.Data;

namespace com.ccvonline.Residency.Model
{
    /// <summary>
    /// 
    /// </summary>
    [Table( "_com_ccvonline_Residency_ProjectPointOfAssessment" )]
    [DataContract]
    public class ProjectPointOfAssessment : com.ccvonline.Data.Model<ProjectPointOfAssessment>
    {
        #region Entity Properties

        /// <summary>
        /// Gets or sets the residency project id.
        /// </summary>
        /// <value>
        /// The residency project id.
        /// </value>
        [Required]
        [DataMember]
        public int ProjectId { get; set; }

        /// <summary>
        /// Gets or sets the residency competency type value id.
        /// </summary>
        /// <value>
        /// The residency competency type value id.
        /// </value>
        [DefinedValue( com.ccvonline.SystemGuid.DefinedType.RESIDENCY_COMPETENCY_TYPE )]
        public int? CompetencyTypeValueId { get; set; }

        /// <summary>
        /// Gets or sets the assessment order.
        /// </summary>
        /// <value>
        /// The assessment order.
        /// </value>
        [Required]
        [DataMember]
        public int AssessmentOrder { get; set; }

        /// <summary>
        /// Gets or sets the assessment text.
        /// </summary>
        /// <value>
        /// The assessment text.
        /// </value>
        [Required]
        [DataMember]
        public string AssessmentText { get; set; }
        
        #endregion

        #region Virtual Properties

        /// <summary>
        /// Gets or sets the residency project.
        /// </summary>
        /// <value>
        /// The residency project.
        /// </value>
        public virtual Project Project { get; set; }

        /// <summary>
        /// Gets or sets the residency competency type value.
        /// </summary>
        /// <value>
        /// The residency competency type value.
        /// </value>
        public virtual Rock.Model.DefinedValue CompetencyTypeValue { get; set; }
        
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
            return string.Format( "Assessment: {0}, Project: {1}", AssessmentText, Project );
        }

        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public partial class ProjectPointOfAssessmentConfiguration : EntityTypeConfiguration<ProjectPointOfAssessment>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectPointOfAssessmentConfiguration" /> class.
        /// </summary>
        public ProjectPointOfAssessmentConfiguration()
        {
            this.HasRequired( a => a.Project ).WithMany(a => a.ProjectPointOfAssessments).HasForeignKey( a => a.ProjectId ).WillCascadeOnDelete( false );
            this.HasOptional( p => p.CompetencyTypeValue ).WithMany().HasForeignKey( p => p.CompetencyTypeValueId ).WillCascadeOnDelete( false );
        }
    }
}
