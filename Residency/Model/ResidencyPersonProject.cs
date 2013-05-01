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
    [Table( "_com_ccvonline_ResidencyPersonProject")]
    [DataContract]
    public class ResidencyPersonProject : ResidencyModel<ResidencyPersonProject>
    {
        #region Entity Properties
        public int PersonId { get; set; }
        public int ResidencyCompetencyPersonId { get; set; }
        public int MinAssignmentCount { get; set; }
        #endregion
    }
}
