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
    [Table( "_com_ccvonline_ResidencyProjectPointOfAssessment" )]
    [DataContract]
    public class ResidencyProjectPointOfAssessment : ResidencyModel<ResidencyProjectPointOfAssessment>
    {
        #region Entity Properties
        public int ResidencyProjectId { get; set; }
        public int AssessmentOrder { get; set; }
        public string AssessmentText { get; set; }
        #endregion
    }
}
