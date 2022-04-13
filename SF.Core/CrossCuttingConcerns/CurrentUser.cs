using System;
using System.Threading;
using SF.Core.Utilities.Security.Web;

namespace SF.Core.CrossCuttingConcerns
{
	/// <summary>
	/// Aktif kullanıcı bilgisini getrir
	/// Login olmamış kullanıcı id -1 gelir
	/// Thread.CurrentPrincipal.Identity den veri gelir 
	/// </summary>
	public class CurrentUser
	{
		public static UserIdentity Identity
		{
			get
			{
				var user = (UserIdentity)Thread.CurrentPrincipal?.Identity;
				if (user != null)
				{
					return user;
				}
				return new UserIdentity()
				{
					IsAuthenticated = false,
					Name = Environment.MachineName,
					UserId = -1,
					UserIp = Environment.MachineName,

				};

			}
		}

		public static int Id => Identity.UserId.HasValue ? Identity.UserId.Value : -1;
		public static string Ip => Identity.UserIp;
		public static string UserName => Identity.Name;



	}
}
