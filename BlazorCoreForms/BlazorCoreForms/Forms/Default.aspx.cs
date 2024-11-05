using System;
using System.Web.UI;
using Serilog;

namespace BlazorCoreForms.Forms
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var logger = Log.ForContext<_Default>();

            logger.Information("Default Webform Page_Load");
        }
    }
}