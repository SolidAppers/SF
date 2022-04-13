using Microsoft.AspNetCore.Http;


namespace SF.Core.CrossCuttingConcerns
{


	public static class WebAppContext
	{
		private static IHttpContextAccessor _httpContextAccessor;

		public static void Configure(IHttpContextAccessor httpContextAccessor)
		{
			_httpContextAccessor = httpContextAccessor;
		}

		public static HttpContext Current => _httpContextAccessor.HttpContext;
	}
}
