using System;
using System.Collections.Generic;
using System.Text;

namespace Tnomer
{
    static public class ErrorExtension
    {
        public static String ToSummary(this Exception ex)
        {
            String s;

            s = ex.Message
                + Environment.NewLine + ex.TargetSite.Name
                + Environment.NewLine + ex.StackTrace;

           return s;
        }
    }
}
