using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tnomer.AspNetCore.Mvc.Models
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
