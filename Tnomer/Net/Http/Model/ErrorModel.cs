using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tnomer.Net.Http.Model
{
    public class ErrorModel
    {
        public String Message;

        public ErrorModel(String message)
        {
            Message = message;
        }
        
    }
}
