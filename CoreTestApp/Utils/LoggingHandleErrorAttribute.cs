﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Imobiliare.UI.BusinessLayer
{
  using System.Web.Mvc;

  public class LoggingHandleErrorAttribute : HandleErrorAttribute
  {
    private readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(LoggingHandleErrorAttribute));

    public override void OnException(ExceptionContext context)
    {
      base.OnException(context);
      //if (context.ExceptionHandled)
      //{
      //Log everything
        log.ErrorFormat("Internal error occured in application: {0}, StackTrace {1}", context.Exception.Message, context.Exception.StackTrace);
      //}
    }
  }
}