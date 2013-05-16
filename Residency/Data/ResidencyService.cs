using Rock.Data;

namespace com.ccvonline.Residency.Data
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResidencyService<T> : Rock.Data.Service<T> where T: Rock.Data.Entity<T>, new()
    {
        public ResidencyService()
            : base(new EFRepository<T>(new ResidencyContext()))
        {
        }
    }
}
