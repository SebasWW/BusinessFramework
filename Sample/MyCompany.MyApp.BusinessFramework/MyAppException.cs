using System;
using System.Collections.Generic;
using System.Text;

namespace MyCompany.MyApp
{
    public class MyAppException:Exception
    {
        internal MyAppException(String message) :base(message){}
    }
}
