using System;

using CoreForms.Web.Infrastructure;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Patterns;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorCoreForms.Extensions
{
	public static class LegacyAspNetEndpointRouteBuilder
	{
		public static IEndpointConventionBuilder MapCoreForms(this IEndpointRouteBuilder endpoints, string pattern)
		{
			if (endpoints == null)
			{
				throw new ArgumentNullException("endpoints");
			}

			if (pattern == null)
			{
				throw new ArgumentNullException("pattern");
			}

			LegacyAspNetOptions requiredService = endpoints.ServiceProvider.GetRequiredService<LegacyAspNetOptions>();
			return endpoints.MapCoreForms(pattern, requiredService);
		}

		public static IEndpointConventionBuilder MapCoreForms(this IEndpointRouteBuilder endpoints, RoutePattern pattern)
		{
			if (endpoints == null)
			{
				throw new ArgumentNullException("endpoints");
			}

			if (pattern == null)
			{
				throw new ArgumentNullException("pattern");
			}

			LegacyAspNetOptions requiredService = endpoints.ServiceProvider.GetRequiredService<LegacyAspNetOptions>();
			return endpoints.MapCoreForms(pattern, requiredService);
		}

		public static IEndpointConventionBuilder MapCoreForms(this IEndpointRouteBuilder endpoints, string pattern, LegacyAspNetOptions options)
		{
			if (endpoints == null)
			{
				throw new ArgumentNullException("endpoints");
			}

			if (pattern == null)
			{
				throw new ArgumentNullException("pattern");
			}

			if (options == null)
			{
				throw new ArgumentNullException("options");
			}

			RequestDelegate requestDelegate = BuildPipeline(endpoints, options);
			return endpoints.Map(pattern, requestDelegate);
		}

		public static IEndpointConventionBuilder MapCoreForms(this IEndpointRouteBuilder endpoints, RoutePattern pattern, LegacyAspNetOptions options)
		{
			if (endpoints == null)
			{
				throw new ArgumentNullException("endpoints");
			}

			if (pattern == null)
			{
				throw new ArgumentNullException("pattern");
			}

			if (options == null)
			{
				throw new ArgumentNullException("options");
			}

			RequestDelegate requestDelegate = BuildPipeline(endpoints, options);
			return endpoints.Map(pattern, requestDelegate);
		}

		private static RequestDelegate BuildPipeline(IEndpointRouteBuilder builder, LegacyAspNetOptions options)
		{
			return builder.CreateApplicationBuilder().UseMiddleware<Middlewares.LegacyAspNetMiddleware>(new object[1] { options }).Build();
		}
	}

}
