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
    [Table( "_com_ccvonline_Residency_CompetencyPersonProject" )]
    [DataContract]
    public class CompetencyPersonProject : Model<CompetencyPersonProject>
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
        public int CompetencyPersonId { get; set; }

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
        public virtual CompetencyPerson CompetencyPerson { get; set; }

        /// <summary>
        /// Gets or sets the residency project.
        /// </summary>
        /// <value>
        /// The residency project.
        /// </value>
        public virtual Project Project { get; set; }

        /// <summary>
        /// Gets or sets the residency competency person project assignments.
        /// </summary>
        /// <value>
        /// The residency competency person project assignments.
        /// </value>
        public virtual List<CompetencyPersonProjectAssignment> CompetencyPersonProjectAssignments { get; set; }

        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public partial class CompetencyPersonProjectConfiguration : EntityTypeConfiguration<CompetencyPersonProject>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CompetencyPersonProjectConfiguration"/> class.
        /// </summary>
        public CompetencyPersonProjectConfiguration()
        {
            this.HasRequired( a => a.CompetencyPerson ).WithMany( a => a.CompetencyPersonProjects ).HasForeignKey( a => a.CompetencyPersonId ).WillCascadeOnDelete( false );
            this.HasRequired( a => a.Project ).WithMany().HasForeignKey( a => a.ProjectId ).WillCascadeOnDelete( false );
        }
    }
}
