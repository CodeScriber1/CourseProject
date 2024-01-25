using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using CourseProject.Service.Exceptions;

namespace CourseProject.API.Middlewares
{
	public class BookStoreExceptionMiddleware
	{
		private readonly RequestDelegate next;
		private readonly ILogger<BookStoreExceptionMiddleware> logger;
		public BookStoreExceptionMiddleware(RequestDelegate next, ILogger<BookStoreExceptionMiddleware> logger)
		{
			this.next = next;
			this.logger = logger;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			try
			{
				await next.Invoke(context);
			}
			catch (BookShopException ex)
			{
				await HandleException(context, ex.Code, ex.Message);
			}
			catch (Exception ex)
			{
				//Log
				logger.LogError(ex.ToString());

				await HandleException(context, 500, ex.Message);
			}
		}

		public async Task HandleException(HttpContext context, int code, string message)
		{
			context.Response.StatusCode = code;
			await context.Response.WriteAsJsonAsync(new
			{
				Code = code,
				Message = message
			});
		}
	}
}
