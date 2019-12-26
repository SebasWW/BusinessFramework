using System;
using System.Collections.Generic;
using System.Text;

namespace MyCompany.MyApp
{
    public class EntryValidationException:MyAppException
    {
        internal EntryValidationException(String message) :base(message){ }

        internal EntryValidationException() : base("Недопустимые данные.") { }
    }
}
