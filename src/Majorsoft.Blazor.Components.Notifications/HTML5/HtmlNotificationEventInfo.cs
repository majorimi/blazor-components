using System;
using System.Threading.Tasks;

using Microsoft.JSInterop;

namespace Majorsoft.Blazor.Components.Notifications
{
	/// <summary>
	/// Html5 Notification Permission Request result event <see cref="DotNetObjectReference"/> info to handle JS callback
	/// </summary>
	internal sealed class HtmlNotificationEventInfo
	{
		private readonly Func<Guid, Task>? _onOpenCallback;
		private readonly Func<Guid, Task>? _onClickCallback;
		private readonly Func<Guid, Task>? _onCloseCallback;
		private readonly Func<Guid, Task>? _onErrorCallback;

		public Guid Id { get; }

		public HtmlNotificationEventInfo(Guid id,
			Func<Guid, Task>? onOpenCallback = null,
			Func<Guid, Task>? onClickCallback = null,
			Func<Guid, Task>? onCloseCallback = null,
			Func<Guid, Task>? onErrorCallback = null)
		{
			Id = id;
			_onOpenCallback = onOpenCallback;
			_onClickCallback = onClickCallback;
			_onCloseCallback = onCloseCallback;
			_onErrorCallback = onErrorCallback;
		}

		[JSInvokable("OnOpen")]
		public async Task OnOpen()
		{
			if (_onOpenCallback is not null)
			{
				await _onOpenCallback(Id);
			}
		}
		[JSInvokable("OnClick")]
		public async Task OnClick()
		{
			if (_onClickCallback is not null)
			{
				await _onClickCallback(Id);
			}
		}
		[JSInvokable("OnClose")]
		public async Task OnClose()
		{
			if (_onCloseCallback is not null)
			{
				await _onCloseCallback(Id);
			}
		}
		[JSInvokable("OnError")]
		public async Task OnError()
		{
			if (_onErrorCallback is not null)
			{
				await _onErrorCallback(Id);
			}
		}
	}
}