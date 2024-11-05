using System;
using System.IO;
using System.Threading.Tasks;

using BlazorCoreForms.Components;
using BlazorCoreForms.Extensions;

using CoreForms.Web.Infrastructure;
using CoreForms.Web.Infrastructure.Authentication;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serilog;

LegacyAspNetInitialization.License = "I hereby confirm that I use CoreForms only for trial purposes and have read and accept the CoreForms Trial License.";

var builder = WebApplication.CreateBuilder();
//builder.WebHost.UseContentRoot(Environment.CurrentDirectory);
//builder.WebHost.UseWebRoot(Path.Combine(Environment.CurrentDirectory, "wwwroot"));

Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger();
builder.Host.UseSerilog();

var logger = Log.ForContext<Program>();

logger.Information("ContentRootPath {CRP}", builder.Environment.ContentRootPath);
logger.Information("WebRootPath {WRP}", builder.Environment.WebRootPath);

logger.Information("Starting application...");

string webFormsPath = Path.Combine(Environment.CurrentDirectory, "Webforms");

logger.Information("CurrentDirectory {CurrentDirectory}", Environment.CurrentDirectory);

logger.Information("webFormsPath {webFormsPath}", webFormsPath);


var legacyAspNetInitializationOptions = new LegacyAspNetInitializationOptions(virtualPath: "/forms/", physicalPath: Path.Combine(Environment.CurrentDirectory, "forms"));

legacyAspNetInitializationOptions = new LegacyAspNetInitializationOptions(legacyAspNetInitializationOptions)
{
	CustomBinDirectory = Path.Combine(Environment.CurrentDirectory, "bin")
};

LegacyAspNetInitialization.Initialize(legacyAspNetInitializationOptions);


builder.Services.AddRazorComponents().AddInteractiveWebAssemblyComponents();

builder.Services.AddLegacyAspNet(options =>
{
	options.AuthenticationSharing = AspNetAuthenticationSharing.Shared;
	options.PipelineCustomizer = new CustomPipelineCustomizer();
});

var app = builder.Build();

try
{
	logger.Information("Building application...");


	if (app.Environment.IsDevelopment())
	{
		app.UseWebAssemblyDebugging();
	}
	else
	{
		app.UseExceptionHandler("/Error", createScopeForErrors: true);
		app.UseHsts();
	}

	//app.UsePathBase("/blazor-iis/");

	app.UseHttpsRedirection();

	app.UseStaticFiles();

	app.UseAntiforgery();


	app.MapRazorComponents<App>()
		.AddInteractiveWebAssemblyRenderMode()
		.AddAdditionalAssemblies(typeof(BlazorCoreForms.Client._Imports).Assembly);

	app.MapGet("/webform", context =>
	{
		context.Response.Redirect("forms/Default.aspx");
		return Task.CompletedTask;
	});

	app.MapLegacyAspNet("forms/{**rest}");

	//app.MapCoreForms("{**rest}");

	app.Run();
}
catch (Exception ex)
{
	Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
	Log.CloseAndFlush();
}