using System.Linq;

namespace SebasWW.BusinessFramework.Security
{
    public abstract class ReadSecurityFilter<TEntry>
    {
        protected abstract IQueryable<TEntry> OnReadSecure(BusinessContext context, IQueryable<TEntry> query);

        internal IQueryable<TEntry> ReadSecure(BusinessContext context, IQueryable<TEntry> query)
        {
            return OnReadSecure(context, query);
        }
    }
}
