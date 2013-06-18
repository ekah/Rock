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

using Rock.Data;

namespace com.ccvonline.Residency.Model
{
    [Table( "_com_ccvonline_Residency_Competency" )]
    [DataContract]
    public class ResidencyCompetency : NamedModel<ResidencyCompetency>
    {
        #region Entity Properties

        /// <summary>
        /// Gets or sets the residency track id.
        /// </summary>
        /// <value>
        /// The residency track id.
        /// </value>
        [Required]
        [DataMember]
        public int ResidencyTrackId { get; set; }

        /// <summary>
        /// Gets or sets the teacher of record person id.
        /// </summary>
        /// <value>
        /// The teacher of record person id.
        /// </value>
        [DataMember]
        public int? TeacherOfRecordPersonId { get; set; }

        /// <summary>
        /// Gets or sets the facilitator person id.
        /// </summary>
        /// <value>
        /// The facilitator person id.
        /// </value>
        [DataMember]
        public int? FacilitatorPersonId { get; set; }

        /// <summary>
        /// Gets or sets the goals.
        /// </summary>
        /// <value>
        /// The goals.
        /// </value>
        [DataMember]
        public string Goals { get; set; }

        /// <summary>
        /// Gets or sets the credit hours.
        /// </summary>
        /// <value>
        /// The credit hours.
        /// </value>
        [DataMember]
        public int? CreditHours { get; set; }

        /// <summary>
        /// Gets or sets the supervision hours.
        /// </summary>
        /// <value>
        /// The supervision hours.
        /// </value>
        [DataMember]
        public int? SupervisionHours { get; set; }

        /// <summary>
        /// Gets or sets the implementation hours.
        /// </summary>
        /// <value>
        /// The implementation hours.
        /// </value>
        [DataMember]
        public int? ImplementationHours { get; set; }

        /// <summary>
        /// Gets or sets the residency competency type value id.
        /// </summary>
        /// <value>
        /// The residency competency type value id.
        /// </value>
        [DefinedValue( com.ccvonline.Residency.SystemGuid.DefinedType.RESIDENCY_COMPETENCY_TYPE )]
        public int? ResidencyCompetencyTypeValueId { get; set; }

        #endregion

        #region Virtual Properties

        /// <summary>
        /// Gets or sets the residency track.
        /// </summary>
        /// <value>
        /// The residency track.
        /// </value>
        public virtual ResidencyTrack ResidencyTrack { get; set; }

        /// <summary>
        /// Gets or sets the teacher of record person.
        /// </summary>
        /// <value>
        /// The teacher of record person.
        /// </value>
        public virtual Rock.Model.Person TeacherOfRecordPerson { get; set; }

        /// <summary>
        /// Gets or sets the facilitator person.
        /// </summary>
        /// <value>
        /// The facilitator person.
        /// </value>
        public virtual Rock.Model.Person FacilitatorPerson { get; set; }

        /// <summary>
        /// Gets or sets the residency projects.
        /// </summary>
        /// <value>
        /// The residency projects.
        /// </value>
        public virtual List<ResidencyProject> ResidencyProjects { get; set; }

        /// <summary>
        /// Gets or sets the residency competency type value.
        /// </summary>
        /// <value>
        /// The residency competency type value.
        /// </value>
        public virtual Rock.Model.DefinedValue ResidencyCompetencyTypeValue { get; set; }

        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public partial class ResidencyCompetencyConfiguration : EntityTypeConfiguration<ResidencyCompetency>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResidencyCompetencyConfiguration"/> class.
        /// </summary>
        public ResidencyCompetencyConfiguration()
        {
            this.HasRequired( p => p.ResidencyTrack ).WithMany( p => p.ResidencyCompetencies ).HasForeignKey( p => p.ResidencyTrackId ).WillCascadeOnDelete( false );
            this.HasOptional( p => p.TeacherOfRecordPerson ).WithMany().HasForeignKey( p => p.TeacherOfRecordPersonId ).WillCascadeOnDelete( false );
            this.HasOptional( p => p.FacilitatorPerson ).WithMany().HasForeignKey( p => p.FacilitatorPersonId ).WillCascadeOnDelete( false );
            this.HasOptional( p => p.ResidencyCompetencyTypeValue ).WithMany().HasForeignKey( p => p.ResidencyCompetencyTypeValueId ).WillCascadeOnDelete( false );
        }
    }
}
