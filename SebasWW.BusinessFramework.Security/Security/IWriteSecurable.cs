using System;
using System.Linq;
using System.Threading.Tasks;

namespace SebasWW.BusinessFramework.Security
{
    public interface IWriteSecurable
    {
        Task<WriteSecurityCheck> CheckSecurityAsQuery(BusinessContext businessManager, bool beforeUpdate);
    }
}
