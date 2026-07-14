using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Majorsoft.Blazor.Components.Media.Capture
{
	/// <summary>
	/// Simple HTML based media capture component: renders an <c>&lt;input type="file"&gt;</c> element
	/// with the <c>accept</c> and <c>capture</c> attributes. On mobile devices the browser opens the
	/// native camera/microphone app, on desktop a file picker. No JS interop or permission handling
	/// is required, the captured media arrives as <see cref="IBrowserFile"/>.
	/// For live in-page camera preview use <see cref="CameraCapture"/> instead.
	/// </summary>
	public partial class MediaFileCapture
	{
		/// <summary>Type of media to capture, rendered as HTML <c>accept</c> attribute. Default is <see cref="CaptureMediaTypes.Photo"/>.</summary>
		[Parameter] public CaptureMediaTypes MediaType { get; set; } = CaptureMediaTypes.Photo;

		/// <summary>
		/// Camera direction preference rendered as HTML <c>capture</c> attribute. <see cref="CameraFacingModes.Default"/>
		/// renders no attribute (the user can pick existing files as well), User/Environment ask for direct capture
		/// with the front/rear camera on mobile devices.
		/// </summary>
		[Parameter] public CameraFacingModes CaptureMode { get; set; } = CameraFacingModes.Default;

		/// <summary>True to allow selecting multiple files. Default is false.</summary>
		[Parameter] public bool Multiple { get; set; }

		/// <summary>Maximum number of files to accept when <see cref="Multiple"/> is set. Default is 10.</summary>
		[Parameter] public int MaxFileCount { get; set; } = 10;

		/// <summary>True to disable the input element.</summary>
		[Parameter] public bool Disabled { get; set; }

		/// <summary>Custom CSS class(es) applied to the input element.</summary>
		[Parameter] public string? Class { get; set; }

		/// <summary>Custom inline style applied to the input element.</summary>
		[Parameter] public string? Style { get; set; }

		/// <summary>Callback fired when the user captured or selected file(s), with the browser file(s) to read.</summary>
		[Parameter] public EventCallback<IReadOnlyList<IBrowserFile>> OnFilesCaptured { get; set; }

		/// <summary>Arbitrary HTML attributes applied to the input element.</summary>
		[Parameter(CaptureUnmatchedValues = true)]
		public Dictionary<string, object>? AdditionalAttributes { get; set; }

		private string? AcceptValue => MediaType switch
		{
			CaptureMediaTypes.Photo => "image/*",
			CaptureMediaTypes.Video => "video/*",
			CaptureMediaTypes.Audio => "audio/*",
			_ => null
		};

		private string? CaptureValue => CaptureMode switch
		{
			CameraFacingModes.User => "user",
			CameraFacingModes.Environment => "environment",
			_ => null
		};

		private async Task OnChangeAsync(InputFileChangeEventArgs args)
		{
			if (OnFilesCaptured.HasDelegate)
			{
				await OnFilesCaptured.InvokeAsync(args.GetMultipleFiles(MaxFileCount));
			}
		}
	}
}
