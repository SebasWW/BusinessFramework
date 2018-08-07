using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Net;
using Newtonsoft.Json;

namespace Tnomer.AspNetCore.Mvc.Models
{
    public class ErrorResponseModel
    {
        [JsonProperty(PropertyName = "metadata")]
        public MetaModel Metadata = new MetaModel();

        [JsonProperty(PropertyName = "errors")]
        public IEnumerable<ErrorModel> Errors;

        public ErrorResponseModel(ModelStateDictionary modelState)
        {
            Errors = ModelStateToErrors(modelState);
        }

        public ErrorResponseModel(IEnumerable<ErrorModel> errors)
        {
            Errors = errors;
        }

        private ErrorModel[] ModelStateToErrors(ModelStateDictionary modelState)
        {
            List<ErrorModel> l = new List<ErrorModel>();

            foreach (var ms in modelState)
            {
                foreach(var err in ms.Value.Errors)
                {
                    l.Add(new ErrorModel(String.Concat( err.ErrorMessage , err?.Exception?.Message)));
                }

            }

            return l.ToArray();
        }
    }
}
