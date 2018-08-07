using System;
using System.Collections.Generic;
using System.IO;

namespace Tnomer.IO
{
    public static class StreamExtension
    {
        public static String ReadToEnd( this Stream s)
        {
            using (var r = new StreamReader(s))
            {
                return r.ReadToEnd();
            }
        }
        public static String ReadToEnd(this Stream s, System.Text.Encoding encoding)
        {
            using (var r = new StreamReader(s, encoding))
            {
                return r.ReadToEnd();
            }
        }
    }
}
