using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SebasWW.BusinessFramework.Security
{
    public abstract class ReadSecurityFilter<TEntry>
    {
        protected abstract IQueryable<TEntry> OnReadSecure(BusinessManager context, IQueryable<TEntry> query);

        internal IQueryable<TEntry> ReadSecure(BusinessManager context, IQueryable<TEntry> query)
        {
            return OnReadSecure(context, query);
        }
    }
}
