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
    [Table( "_com_ccvonline_ResidencyProjectPointOfAssessment" )]
    [DataContract]
    public class ResidencyProjectPointOfAssessment : ResidencyModel<ResidencyProjectPointOfAssessment>
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
        public int ResidencyProjectId { get; set; }

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
        public virtual ResidencyProject ResidencyProject { get; set; }
        
        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public partial class ResidencyProjectPointOfAssessmentConfiguration : EntityTypeConfiguration<ResidencyProjectPointOfAssessment>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResidencyProjectPointOfAssessmentConfiguration" /> class.
        /// </summary>
        public ResidencyProjectPointOfAssessmentConfiguration()
        {
            this.HasRequired( a => a.ResidencyProject ).WithMany(a => a.ResidencyProjectPointOfAssessments).HasForeignKey( a => a.ResidencyProjectId ).WillCascadeOnDelete( false );
        }
    }
}
