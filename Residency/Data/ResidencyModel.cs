using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace com.ccvonline.Residency.Data
{
    public class ResidencyModel<T>
    {
        #region Properties

        /// <summary>
        /// The Id
        /// </summary>
        [Key]
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Gets or 
        /// </summary>
        /// <value>
        /// The GUID.
        /// </value>
        [Rock.Data.AlternateKey]
        [DataMember]
        public Guid Guid
        {
            get { return _guid; }
            set { _guid = value; }
        }
        private Guid _guid = Guid.NewGuid();

        #endregion
    }
}
