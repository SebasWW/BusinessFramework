using System;
using System.Collections.Generic;
using System.Text;

namespace Tnomer.BusinessFramework
{
    internal interface IBusinessObject
    {
        void ValuesCheck();
        void ConsistencyCheck(Boolean beforeUpdate);
        void PermissionWriteCheck(Boolean beforeUpdate);
    }
}
