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
    [Table("_com_ccvonline_ResidencyPersonProjectAssignment")]
    [DataContract]
    public class ResidencyPersonProjectAssignment : ResidencyModel<ResidencyPersonProjectAssignment>
    {
        #region Entity Properties

        public int ResidencyPersonProjectId { get; set; }

        public int AssessorPersonId { get; set; }

        public DateTime CompletedDateTime { get; set; }

        #endregion

        #region Virtual Properties

        public ResidencyPersonProject ResidencyPersonProject { get; set; }
        #endregion
    }
}
