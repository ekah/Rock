using System.Linq;
using com.ccvonline.CommandCenter.Model;
using Rock.Data;

namespace com.ccvonline.CommandCenter.Data
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CommandCenterService<T> : Rock.Data.Service<T> where T : Rock.Data.Entity<T>, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandCenterService{T}"/> class.
        /// </summary>
        public CommandCenterService()
            : base( new EFRepository<T>( new CommandCenterContext() ) )
        {
        }
    }
}
