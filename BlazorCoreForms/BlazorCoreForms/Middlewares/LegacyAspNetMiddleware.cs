using System;
using System.Threading.Tasks;
using System.Web;

using CoreForms.Web.Infrastructure;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;

namespace BlazorCoreForms.Middlewares;

public class LegacyAspNetMiddleware
{
	private readonly RequestDelegate next;
	private readonly LegacyAspNetOptions _options;

	public LegacyAspNetMiddleware(RequestDelegate requestDelegate, LegacyAspNetOptions options)
	{
		next = requestDelegate ?? throw new ArgumentNullException(nameof(requestDelegate));
		_options = options ?? throw new ArgumentNullException(nameof(options));
	}

	public async Task Invoke(Microsoft.AspNetCore.Http.HttpContext context)
	{
		var path = context.Request.Path.Value;
		if (path.EndsWith(".aspx", StringComparison.OrdinalIgnoreCase) ||
			path.EndsWith(".ashx", StringComparison.OrdinalIgnoreCase) ||
			path.EndsWith(".ascx", StringComparison.OrdinalIgnoreCase) ||
			path.EndsWith(".asax", StringComparison.OrdinalIgnoreCase))
		{
			IHttpBodyControlFeature httpBodyControlFeature = context.Features.Get<IHttpBodyControlFeature>();
			bool allowSynchronousIoBackup = false;
			if (httpBodyControlFeature != null)
			{
				allowSynchronousIoBackup = httpBodyControlFeature.AllowSynchronousIO;
				httpBodyControlFeature.AllowSynchronousIO = true;
			}

			AspNetCoreHttpWorkerRequest aspNetCoreHttpWorkerRequest = new AspNetCoreHttpWorkerRequest(context, _options);
			try
			{
				HttpRuntime.ProcessRequest(aspNetCoreHttpWorkerRequest);
				await aspNetCoreHttpWorkerRequest.Task;
			}
			finally
			{
				if (httpBodyControlFeature != null)
				{
					httpBodyControlFeature.AllowSynchronousIO = allowSynchronousIoBackup;
				}
			}
		}
		else
		{
			await next(context);
		}
	}
}
