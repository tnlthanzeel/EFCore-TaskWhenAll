using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace GemSto.Web
{
    public class LoginLog: ActionFilterAttribute
    {
        //public override void OnActionExecuting(ActionExecutingContext context)
        //{
        //    _logger.LogWarning("ClassFilter OnActionExecuting");
        //    base.OnActionExecuting(context);
        //}

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            Debug.Write("writing to auit log");
        }

        //public override void OnResultExecuting(ResultExecutingContext context)
        //{
        //    _logger.LogWarning("ClassFilter OnResultExecuting");
        //    base.OnResultExecuting(context);
        //}

        //public override void OnResultExecuted(ResultExecutedContext context)
        //{
        //    _logger.LogWarning("ClassFilter OnResultExecuted");
        //    base.OnResultExecuted(context);
        //}
    }
}
