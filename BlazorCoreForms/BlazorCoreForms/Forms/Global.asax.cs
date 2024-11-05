using System;
using System.Web;

using Serilog;

namespace BlazorCoreForms.Forms;

public class Global : HttpApplication
{
	protected void Application_Start(Object sender, EventArgs e)
	{
		var logger = Log.ForContext<Global>();

		logger.Information("-----------------------------Application Start -----------------------------");
	}
}