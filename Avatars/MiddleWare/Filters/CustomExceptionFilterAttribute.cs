using Avatars.Utils.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Filters
{
	//public class CustomExceptionFilterAttribute : Attribute, IExceptionFilter
	//public void OnException(ExceptionContext context)
	public class CustomExceptionFilterAttribute : ExceptionFilterAttribute
	{
		protected readonly ILogger<CustomExceptionFilterAttribute> Logger;

		public CustomExceptionFilterAttribute(ILogger<CustomExceptionFilterAttribute> logger)
		{
			Logger = logger;
		}

		public override void OnException(ExceptionContext context)
		{
			Logger.LogError(context.Exception, context.ActionDescriptor.DisplayName);

			Result(Error(
				context.Exception.GetType().Name, 
				context.Exception.Message,
				context.ActionDescriptor.DisplayName),
				statusCode: 500);

			context.ExceptionHandled = true;
		}

		JsonResult Result(object o, int statusCode) => new JsonResult(o) { StatusCode = statusCode };

		ErrorMessage Error(string exceptionName, string exceptionMessage, string actionName) 
			=> new ErrorMessage(exceptionName, exceptionMessage, actionName);	

		class ErrorMessage
		{
			public string ExceptionName { get; set; }
			public string ExceptionMessage { get; set; }
			public string ActionName { get; set; }

			public ErrorMessage(string exceptionName, string exceptionMessage, string actionName)
			{
				ExceptionName = exceptionName;
				ExceptionMessage = exceptionMessage;
				ActionName = actionName;
			}
		}

		

	}

}
