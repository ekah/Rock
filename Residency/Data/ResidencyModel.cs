using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace com.ccvonline.Residency.Data
{
    public class ResidencyModel<T> : Rock.Data.Model<T> where T : Rock.Data.Model<T>, Rock.Security.ISecured, new()
    {
    }
}
