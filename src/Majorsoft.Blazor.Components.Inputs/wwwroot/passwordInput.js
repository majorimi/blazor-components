// Majorsoft Blazor PasswordInput JS interop.
// All masking/value logic lives in C# (so it stays testable); JS only reads the caret position after
// an edit and restores it after Blazor re-renders the masked value (which would jump the caret to the end).

// Returns the current caret (selectionStart) of the input so C# can reconstruct the real value.
export function getCaret(input) {
	if (!input) {
		return null;
	}

	return input.selectionStart;
}

// Restores focus and the caret to the given position. Called from OnAfterRenderAsync once Blazor has
// written the masked value to the DOM, so typing in the middle doesn't make the caret jump to the end.
export function setCaret(input, pos) {
	if (!input) {
		return;
	}

	input.focus();
	try {
		input.setSelectionRange(pos, pos);
	} catch {
		// Ignore: element may have been detached between render and this call.
	}
}

export function focusInput(input) {
	if (input) {
		input.focus();
	}
}
