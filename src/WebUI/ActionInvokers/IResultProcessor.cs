﻿using System;
using System.Web.Mvc;
using Guidelines.Core;
using Guidelines.Core.Commands;
using Guidelines.WebUI.Errors;

namespace Guidelines.WebUI.ActionInvokers
{
    public interface IResultProcessor<TActionMethodResult, TExecutedCommandResult>
        where TActionMethodResult : IActionMethodResult
        where TExecutedCommandResult : CommandResult
    {
        TExecutedCommandResult ProcessCommand(TActionMethodResult actionMethodResult, IGenericMapper mapper);
        Func<IGenericMapper, ErrorContext, ActionResult> HandleSuccess(TExecutedCommandResult executedCommandResult, TActionMethodResult actionMethodResult);
    }
}