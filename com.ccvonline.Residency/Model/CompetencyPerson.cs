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
    [Table("_com_ccvonline_Residency_CompetencyPerson")]
    [DataContract]
    public class CompetencyPerson : Model<CompetencyPerson>
    {
        #region Entity Properties

        /// <summary>
        /// Gets or sets the residency competency id.
        /// </summary>
        /// <value>
        /// The residency competency id.
        /// </value>
        [Required]
        [DataMember]
        public int CompetencyId { get; set; }

        /// <summary>
        /// Gets or sets the person id.
        /// </summary>
        /// <value>
        /// The person id.
        /// </value>
        [Required]
        [DataMember]
        public int PersonId { get; set; }

        #endregion

        #region Virtual Properties

        /// <summary>
        /// Gets or sets the residency competency.
        /// </summary>
        /// <value>
        /// The residency competency.
        /// </value>
        public virtual Competency Competency { get; set; }

        /// <summary>
        /// Gets or sets the person.
        /// </summary>
        /// <value>
        /// The person.
        /// </value>
        public virtual Rock.Model.Person Person { get; set; }

        /// <summary>
        /// Gets or sets the residency competency person projects.
        /// </summary>
        /// <value>
        /// The residency competency person projects.
        /// </value>
        public virtual List<CompetencyPersonProject> CompetencyPersonProjects { get; set; }

        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public class CompetencyPersonConfiguration : EntityTypeConfiguration<CompetencyPerson>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CompetencyPersonConfiguration"/> class.
        /// </summary>
        public CompetencyPersonConfiguration()
        {
            this.HasRequired( p => p.Competency ).WithMany().HasForeignKey( p => p.CompetencyId ).WillCascadeOnDelete( false );
            this.HasRequired( p => p.Person ).WithMany().HasForeignKey( p => p.PersonId ).WillCascadeOnDelete( false );
        }
    }
}
