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
    [Table( "_com_ccvonline_ResidencyPersonProjectAssignmentAssessmentPointOfAssessment")]
    [DataContract]
    public class ResidencyPersonProjectAssignmentAssessmentPointOfAssessment : ResidencyModel<ResidencyPersonProjectAssignmentAssessmentPointOfAssessment>
    {
        #region Entity Properties
        public int ResidencyPersonProjectAssignmentAssessmentId { get; set; }
        public int ResidencyProjectPointOfAssessmentId { get; set; }
        public int? Rating { get; set; }
        public string RatingNotes { get; set; }
        #endregion
    }
}
