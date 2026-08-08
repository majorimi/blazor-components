using System;
using System.Linq;

namespace Majorsoft.Blazor.Components.Canvas
{
	/// <summary>
	/// Options of <c>StartInputCaptureAsync</c> controlling which inputs are tracked on the canvas.
	/// Property names are serialized camelCased for the JS module.
	/// </summary>
	public class CanvasInputOptions
	{
		/// <summary>
		/// Track keyboard state (keydown/keyup on the canvas). Requires the canvas to be focusable:
		/// set the <c>TabIndex</c> parameter and focus it (click or <c>FocusAsync</c>). Default is true.
		/// </summary>
		public bool Keyboard { get; set; } = true;

		/// <summary>Track mouse buttons, position (in drawing buffer pixels), movement and wheel. Default is true.</summary>
		public bool Mouse { get; set; } = true;

		/// <summary>Poll connected gamepads on every input snapshot. Default is true.</summary>
		public bool Gamepads { get; set; } = true;

		/// <summary>
		/// Call <c>preventDefault()</c> on captured key, wheel and context menu events so arrows/space do not
		/// scroll the page and right click is usable as custom input. Default is true.
		/// </summary>
		public bool PreventDefault { get; set; } = true;
	}

	/// <summary>
	/// Snapshot of the input state of a canvas, read with <c>GetInputStateAsync</c> or delivered with every
	/// render loop frame in <see cref="CanvasFrameEventArgs.Input"/> while input capture is active.
	/// Movement and wheel deltas accumulate between snapshots and reset when read.
	/// </summary>
	public class CanvasInputState
	{
		/// <summary>Physical key codes currently held down, e.g. "KeyW", "ArrowLeft", "Space" (KeyboardEvent.code, layout independent).</summary>
		public string[] Keys { get; set; } = Array.Empty<string>();

		/// <summary>
		/// Physical key codes pressed since the previous snapshot (fire-once, no repeats). Unlike <see cref="Keys"/>
		/// a short tap is never missed even when press and release both happen between two frames.
		/// Use for discrete actions (jump, clear, toggle), use <see cref="Keys"/> for continuous ones (movement).
		/// </summary>
		public string[] PressedKeys { get; set; } = Array.Empty<string>();

		/// <summary>Mouse buttons currently held down: 0 left, 1 middle, 2 right.</summary>
		public int[] Buttons { get; set; } = Array.Empty<int>();

		/// <summary>Mouse X position in drawing buffer pixels.</summary>
		public double MouseX { get; set; }

		/// <summary>Mouse Y position in drawing buffer pixels.</summary>
		public double MouseY { get; set; }

		/// <summary>Accumulated relative mouse X movement since the previous snapshot (useful with pointer lock).</summary>
		public double MovementX { get; set; }

		/// <summary>Accumulated relative mouse Y movement since the previous snapshot (useful with pointer lock).</summary>
		public double MovementY { get; set; }

		/// <summary>Accumulated wheel delta since the previous snapshot (positive scrolls down).</summary>
		public double WheelDelta { get; set; }

		/// <summary>
		/// All mouse positions recorded while a button was held since the previous snapshot, as flat x,y pairs
		/// in drawing buffer pixels. Fast strokes lose no points between two frames — connect these for smooth
		/// freehand drawing instead of only <see cref="MouseX"/>/<see cref="MouseY"/>.
		/// </summary>
		public double[] MousePath { get; set; } = Array.Empty<double>();

		/// <summary>True while pointer lock is active on the canvas.</summary>
		public bool IsPointerLocked { get; set; }

		/// <summary>State of all connected gamepads (polled, the Gamepad API has no events).</summary>
		public GamepadState[] Gamepads { get; set; } = Array.Empty<GamepadState>();

		/// <summary>Returns whether the given physical key is held down, e.g. <c>IsKeyDown("KeyW")</c>.</summary>
		public bool IsKeyDown(string code) => Keys.Contains(code);

		/// <summary>Returns whether the given physical key was pressed since the previous snapshot (fire-once).</summary>
		public bool WasKeyPressed(string code) => PressedKeys.Contains(code);

		/// <summary>Returns whether the given mouse button is held down: 0 left, 1 middle, 2 right.</summary>
		public bool IsButtonDown(int button) => Buttons.Contains(button);
	}

	/// <summary>State of one connected gamepad.</summary>
	public class GamepadState
	{
		/// <summary>Index of the gamepad in the browser gamepad list.</summary>
		public int Index { get; set; }

		/// <summary>Device identification string.</summary>
		public string Id { get; set; } = "";

		/// <summary>Axis values (-1..1), typically 0/1 left stick X/Y, 2/3 right stick X/Y.</summary>
		public double[] Axes { get; set; } = Array.Empty<double>();

		/// <summary>Button values (0..1, analog triggers report fractions), standard mapping: 0 A, 1 B, 2 X, 3 Y, ...</summary>
		public double[] Buttons { get; set; } = Array.Empty<double>();

		/// <summary>Returns whether the given button is pressed (value above 0.5).</summary>
		public bool IsPressed(int button) => button >= 0 && button < Buttons.Length && Buttons[button] > 0.5;
	}
}
