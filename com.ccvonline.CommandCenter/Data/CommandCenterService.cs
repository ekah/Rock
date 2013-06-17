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

        /// <summary>
        /// Determines whether this instance can delete the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <returns>
        ///   <c>true</c> if this instance can delete the specified item; otherwise, <c>false</c>.
        /// </returns>
        public bool CanDelete( Recording item, out string errorMessage )
        {
            errorMessage = string.Empty;

            return true;
        }

    }
}
