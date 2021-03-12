using System;

namespace Majorsoft.Blazor.Extensions.BrowserStorage
{
	/// <summary>
	/// HTTP Cookie class.
	/// </summary>
	public class Cookie
	{
		/// <summary>
		/// Gets or sets the name for the Cookie.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the System.Net.Cookie.Value for the Cookie.
		/// </summary>
		public string Value { get; set; }

		/// <summary>
		/// Gets or sets the security level of a Cookie.
		/// </summary>
		public bool Secure { get; set; }

		/// <summary>
		/// Gets or sets the URI for which the Cookie is valid.
		/// </summary>
		public string Domain { get; set; }

		/// <summary>
		/// Gets or sets a list of TCP ports that the Cookie applies to.
		/// </summary>
		public string Port { get; set; }

		/// <summary>
		/// Gets or sets the URIs to which the Cookie applies.
		/// </summary>
		public string Path { get; set; } = "/";

		/// <summary>
		/// Determines whether a page script or other active content can access this cookie.
		/// </summary>
		public bool HttpOnly { get; set; }

		/// <summary>
		/// Gets or sets the expiration date and time for the Cookie
		/// </summary>
		public double? Expires { get; set; }
		/// <summary>
		/// Gets the expiration date and time for the Cookie as a System.DateTime.
		/// </summary>
		public DateTime ExpiresAt
		{
			get
			{
				try
				{
					if (Expires.HasValue)
					{
						return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(Expires.Value);
					}

					return DateTime.MinValue;
				}
				catch (ArgumentOutOfRangeException)
				{
					return DateTime.MaxValue;
				}
			}
		}

		/// <summary>
		///  Gets or sets Cookie SameSite which allows you to declare if your cookie should be 
		///  restricted to a first-party or same-site context.
		///  Values should be "strict", "lax", "none".
		/// </summary>
		public string SameSite { get; set; } = "strict";

		/// <summary>
		/// Converts given DateTime to Unix date.
		/// </summary>
		/// <param name="date">DateTime value to convert</param>
		/// <returns>Total second of Unix date</returns>
		public static double ConvertToUnixDate(DateTime date)
		{
			DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
			TimeSpan diff = date.ToUniversalTime() - origin;
			return Math.Floor(diff.TotalSeconds);
		}
	}
}