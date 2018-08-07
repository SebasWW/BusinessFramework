//using System;
//using System.Collections.Generic;
//using System.Linq;
//using Microsoft.AspNetCore.Mvc.ModelBinding;
//using System.Net;

//namespace Tnomer.Net.Http.Model
//{
//    public class ErrorResponseModel
//    {
//        public MetaModel Metadata = new MetaModel();
//        public ErrorModel[] Errors;

//        public ErrorResponseModel(ModelStateDictionary modelState)
//        {
//            Errors = ModelStateToErrors(modelState);
//        }

//        public ErrorResponseModel(ErrorModel[] errors)
//        {
//            Errors = errors;
//        }

//        private ErrorModel[] ModelStateToErrors(ModelStateDictionary modelState)
//        {
//            List<ErrorModel> l = new List<ErrorModel>();

//            foreach (var ms in modelState)
//            {
//                foreach(var err in ms.Value.Errors)
//                {
//                    l.Add(new ErrorModel(String.Concat( err.ErrorMessage , err?.Exception?.Message)));
//                }

//            }

//            return l.ToArray();
//        }
//    }
//}
