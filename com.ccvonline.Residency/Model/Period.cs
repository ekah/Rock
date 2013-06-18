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
    [Table( "_com_ccvonline_Residency_Period" )]
    [DataContract]
    public partial class Period : NamedModel<Period>
    {
        #region Entity Properties

        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        /// <value>
        /// The start date.
        /// </value>
        [DataMember]
        [Column(TypeName="Date")]
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date.
        /// </summary>
        /// <value>
        /// The end date.
        /// </value>
        [DataMember]
        [Column( TypeName = "Date" )]
        public DateTime? EndDate { get; set; }

        #endregion

        #region Virtual Properties

        /// <summary>
        /// Gets or sets the residency tracks.
        /// </summary>
        /// <value>
        /// The residency tracks.
        /// </value>
        public virtual List<Track> Tracks { get; set; }

        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public partial class PeriodConfiguration : EntityTypeConfiguration<Period>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PeriodConfiguration"/> class.
        /// </summary>
        public PeriodConfiguration()
        {
        }
    }
}
