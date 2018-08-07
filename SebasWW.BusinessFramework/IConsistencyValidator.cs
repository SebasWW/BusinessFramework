using System;
using System.Linq;

namespace SebasWW.BusinessFramework
{
    public interface IConsistencyValidator
    {
        IQueryable<ConsistencyErrorEntry> CheckAsQuery(Boolean beforeUpdate);
    }
}
