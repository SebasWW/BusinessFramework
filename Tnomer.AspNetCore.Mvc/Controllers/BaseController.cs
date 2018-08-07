using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Microsoft.Extensions.Logging;
using Tnomer.AspNetCore.Mvc.Configuration;
using Tnomer.AspNetCore.Mvc.Logging;
using Tnomer.AspNetCore.Mvc;
using Tnomer.AspNetCore.Mvc.Models;
using Tnomer.IO;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;
// Helper to enable request stream rewinds
using Microsoft.AspNetCore.Http.Internal;
using Tnomer.Data.Common.Repository;

namespace Tnomer.AspNetCore.Mvc.Controllers
{

    public abstract class BaseController : Controller 
    {
        public BaseController()
        {
            ControllerName = GetType().Name;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var log = ApplicationLog.Current.ControllerRequestLog;
            if(log != null)
            {
                var s = new StringBuilder();

                var req = context.HttpContext.Request;

                s.Append(
                        req.Protocol + " "
                        + req.Method + " "
                        + req.Host
                        + req.Path
                        + req.QueryString + Environment.NewLine
                    );

                req.Headers.ToList().ForEach(
                        a => a.Value.ToList().ForEach( b => s.Append( a.Key + " " + b + Environment.NewLine))
                    );

                req.EnableRewind();
                req.Body.Position = 0;

                s.Append(
                        req.Body.ReadToEnd() + Environment.NewLine
                    );

                log.LogInformation(s.ToString());
            }

            base.OnActionExecuting(context);
        }


        public ResponseModel<String> GetOkModel() => new ResponseModel<String>();

        public object objLock = new Object();


        ////StringValues includes;
        //private RepositoryRequestParams _requestParams;
        //public T GetRequestParameters<T>() where T: RepositoryRequestParams, new()
        //{
        //        lock (objLock)
        //        {

                    
        //        }

        //        return _requestParams as T;
        //}


        //StringValues includes;
        IEnumerable<String> includes;
        public IEnumerable<String> IncludedResources
        {
            get
            {
                lock (objLock)
                {

                    if (includes == StringValues.Empty) includes = HttpContext.Request.Query
                            .Where(p => p.Key == "include").Select(v => v.Value)
                            .FirstOrDefault().ToString()
                            .ToLower().Split(',', StringSplitOptions.RemoveEmptyEntries);
                }
                return includes;
            }
        }
        //StringValues includes;
        IEnumerable<String> sorters;
        public IEnumerable<String> OrderedResources
        {
            get
            {
                lock (objLock)
                {

                    if (sorters == StringValues.Empty) sorters = HttpContext.Request.Query
                            .Where(p => p.Key == "orderby").Select(v => v.Value)
                            .FirstOrDefault().ToString()
                            .ToLower().Split(',', StringSplitOptions.RemoveEmptyEntries);
                }
                return sorters;
            }
        }

        protected string ControllerName { get ; private set; }

        public String GetQueryParameter(String name)
        {
            return HttpContext.Request.Query
                        .Where(p => p.Key == name).Select(v => v.Value)
                        .FirstOrDefault().ToString();
        }


        private string GetErrorText(Exception ex)
        {
            string str;
            if (ex.InnerException != null)
                str = Environment.NewLine + "    " + GetErrorText(ex.InnerException);
            else
                str = "";
            return ex.Message + str ;
        }

        protected virtual internal  IActionResult ErrorToActionResult(Exception e)
        {
            switch (e)
            {
                case SecurityTokenExpiredException ex:
                    ApplicationLog.Current.ControllerErrorLog.LogError(ControllerName + ": " + ex.Message);
                    return new ForbidResult("TNOMER");

                case ModelValidationException ex:
                    ApplicationLog.Current.ControllerErrorLog.LogWarning(ControllerName + ": " + ex.Message + " " + ex.ModelStateDescription );
                    return new BadRequestObjectResult(new ErrorResponseModel(ex.ModelState));// ex.ModelState);

                case UserValidationException ex:
                    ApplicationLog.Current.ControllerErrorLog.LogCritical(ControllerName + ": " + ex.Message);
                    return new BadRequestObjectResult(
                        new ErrorResponseModel( new ErrorModel[] {new ErrorModel(ex.Message)})
                    );

                case InvalidOperationException ex:
                    ApplicationLog.Current.ControllerErrorLog.LogError(ControllerName + ": " + ex.Message);
                    return new BadRequestResult();

                case NotFoundException ex:
                    ApplicationLog.Current.ControllerErrorLog.LogError(ControllerName + ": " + ex.Message);
                    return new NotFoundResult();


                case Exception ex:
                    ApplicationLog.Current.ControllerErrorLog.LogCritical(ControllerName + ": " + ex.GetType().Name + ": " + GetErrorText(ex));
                    return new StatusCodeResult(500);   // Service Unavailable;

                default:
                case null:
                    ApplicationLog.Current.ControllerErrorLog.LogCritical("Message is null.");
                    throw new ArgumentNullException(nameof(e));
            }
        }
    }
}