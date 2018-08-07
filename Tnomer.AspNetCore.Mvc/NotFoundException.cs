using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tnomer.AspNetCore.Mvc
{
    public class NotFoundException : ApplicationException
    {
        public NotFoundException(String message) :base(message){ }
        public NotFoundException() : base("Не найдено.") { }
    }
}
