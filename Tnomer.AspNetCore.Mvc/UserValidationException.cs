using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tnomer.AspNetCore.Mvc
{
    public class UserValidationException:ApplicationException
    {
        public UserValidationException(String message) :base(message){ }

        public UserValidationException() : base("Ошибка проверки пользователя.") { }
    }
}
