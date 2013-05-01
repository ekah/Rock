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
    [Table( "_com_ccvonline_ResidencyPersonProjectAssignmentAssessment")]
    [DataContract]
    public class ResidencyPersonProjectAssignmentAssessment : ResidencyModel<ResidencyPersonProjectAssignmentAssessment>
    {
        #region Entity Properties
        public int ResidencyPersonProjectAssignmentId { get; set; }
        public DateTime? AssessmentDateTime { get; set; }
        public int? Rating { get; set; }
        public string RatingNotes { get; set; }
        #endregion
    }
}
