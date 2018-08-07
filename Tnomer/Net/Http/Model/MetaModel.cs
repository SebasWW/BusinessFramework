using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Tnomer.Net.Http.Model
{
    public class MetaModel
    {
        public DateTime ExecutedDate { get; set; } = DateTime.Now;
        public DateTime LastModified { get; set; } = DateTime.Now;
    }
}
