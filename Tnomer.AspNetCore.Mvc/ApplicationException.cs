using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tnomer.AspNetCore.Mvc
{
    public class ApplicationException:Exception
    {
        internal ApplicationException(String message) :base(message){ }
    }
}
