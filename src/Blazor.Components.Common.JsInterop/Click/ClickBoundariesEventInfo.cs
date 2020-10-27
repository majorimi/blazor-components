using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace Blazor.Components.Common.JsInterop.Click
{
	/// <summary>
	/// ClickBoundariesEventInfo event <see cref="DotNetObjectReference"/> info to handle JS callback
	/// </summary>
	internal sealed class ClickBoundariesEventInfo
	{
		private readonly Func<MouseEventArgs, Task> _outsideClickCallback;
		private readonly Func<MouseEventArgs, Task> _insideClickCallback;

		public ClickBoundariesEventInfo(Func<MouseEventArgs, Task> outsideClickCallback = null, Func<MouseEventArgs, Task> insideClickCallback = null)
		{
			_outsideClickCallback = outsideClickCallback;
			_insideClickCallback = insideClickCallback;
		}

		[JSInvokable("ClickOutside")]
		public async Task ClickOutside(MouseEventArgs args)
		{
			if (_outsideClickCallback is not null)
			{
				await _outsideClickCallback.Invoke(args);
			}
		}

		[JSInvokable("ClickInside")]
		public async Task ClickInside(MouseEventArgs args)
		{
			if (_insideClickCallback is not null)
			{
				await _insideClickCallback.Invoke(args);
			}
		}
	}
}