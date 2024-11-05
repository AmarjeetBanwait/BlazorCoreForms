using System.Threading.Tasks;
using System.Web;

namespace BlazorCoreForms.Extensions;

public class CustomPipelineCustomizer : IHttpApplicationPipelineCustomizer
{
	public Task BeforePipeline(HttpContext context)
	{
		// Custom logic before the pipeline starts
		return Task.CompletedTask;
	}

	public Task BeforeBeginRequestEvent(HttpContext context)
	{

		return Task.CompletedTask;
	}

	public Task BeforeAuthenticateEvent(HttpContext context)
	{
		// Custom logic before the Authenticate event
		return Task.CompletedTask;
	}

	public Task BeforeAuthorizeEvent(HttpContext context)
	{
		// Custom logic before the Authorize event
		return Task.CompletedTask;
	}

	public Task BeforeResolveRequestCacheEvent(HttpContext context)
	{
		// Custom logic before the ResolveRequestCache event
		return Task.CompletedTask;
	}

	public Task AfterPipeline(HttpContext context)
	{
		// Custom logic after the pipeline ends
		return Task.CompletedTask;
	}
}