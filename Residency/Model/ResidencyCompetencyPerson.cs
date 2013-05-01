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
    [Table("_com_ccvonline_ResidencyCompetencyPerson")]
    [DataContract]
    public class ResidencyCompetencyPerson : ResidencyModel<ResidencyCompetencyPerson>
    {
        #region Entity Properties

        /// <summary>
        /// Gets or sets the residency competency id.
        /// </summary>
        /// <value>
        /// The residency competency id.
        /// </value>
        [DataMember]
        public int ResidencyCompetencyId { get; set; }

        /// <summary>
        /// Gets or sets the person id.
        /// </summary>
        /// <value>
        /// The person id.
        /// </value>
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
        public virtual ResidencyCompetency ResidencyCompetency { get; set; }

        /// <summary>
        /// Gets or sets the person.
        /// </summary>
        /// <value>
        /// The person.
        /// </value>
        public virtual Rock.Model.Person Person { get; set; }

        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public class ResidencyCompetencyPersonConfiguration : EntityTypeConfiguration<ResidencyCompetencyPerson>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResidencyCompetencyPersonConfiguration"/> class.
        /// </summary>
        public ResidencyCompetencyPersonConfiguration()
        {
            this.HasRequired( p => p.ResidencyCompetency ).WithMany().HasForeignKey( p => p.ResidencyCompetencyId ).WillCascadeOnDelete( false );
            this.HasRequired( p => p.Person ).WithMany().HasForeignKey( p => p.PersonId ).WillCascadeOnDelete( false );
        }
    }
}
