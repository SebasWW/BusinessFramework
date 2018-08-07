using System;
using System.Text;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Tnomer.AspNetCore.Mvc
{
    public class ModelValidationException : ApplicationException
    { 
        public ModelStateDictionary ModelState { get; private set; }
        public String ModelStateDescription { get; private set; }

        public ModelValidationException(String message, ModelStateDictionary modelState) :base(message)
        {
            ModelState = modelState;
            SetModelStateErrors();
        }

        public ModelValidationException(ModelStateDictionary modelState) : base("Ошибка проверки данных.")
        {
            ModelState = modelState;
            SetModelStateErrors();
        }

        private void SetModelStateErrors()
        {
            StringBuilder sb= new StringBuilder();

            foreach (var ms in ModelState)
            {
                foreach (var err in ms.Value.Errors)
                {
                    sb.Append(String.Concat(err.ErrorMessage, err.Exception?.Message) + " ");
                }

            }

            ModelStateDescription = sb.ToString();
        }
    }
}
