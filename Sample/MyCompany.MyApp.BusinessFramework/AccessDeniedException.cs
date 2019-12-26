using System;
using System.Collections.Generic;
using System.Text;

namespace MyCompany.MyApp
{
    public class AccessDeniedException : MyAppException
    {
        internal AccessDeniedException(String message) :base(message){ }

        internal AccessDeniedException() : base("Access denied.") { }
    }
}
