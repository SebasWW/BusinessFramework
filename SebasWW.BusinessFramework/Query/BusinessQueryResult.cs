using System;
using System.Collections.Generic;
using System.Text;

namespace Tnomer.BusinessFramework.Query
{
    public class BusinessQueryResult<TResult>
    {
        internal TResult Result{get; set;}

        public BusinessQueryResult(TResult result)
        {
            Result = result;
        }
    }
}
