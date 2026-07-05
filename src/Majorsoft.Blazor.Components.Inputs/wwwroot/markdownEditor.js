// Majorsoft Blazor Markdown editor JS interop.
// The editor keeps all text-transformation logic in C# (so it stays testable); JS only reads the
// current selection from the <textarea> and restores the caret/selection after Blazor re-renders.

// Returns the current selection together with the textarea value so C# can compute the edit.
export function getSelectionInfo(textarea) {
	if (!textarea) {
		return null;
	}

	return {
		start: textarea.selectionStart,
		end: textarea.selectionEnd,
		value: textarea.value,
	};
}

// Restores focus and the given selection range. Called from OnAfterRenderAsync once Blazor has
// written the new value to the DOM, so toolbar edits don't make the caret jump to the end.
export function setSelection(textarea, start, end) {
	if (!textarea) {
		return;
	}

	textarea.focus();
	try {
		textarea.setSelectionRange(start, end);
	} catch {
		// Ignore: element may have been detached between render and this call.
	}
}

export function focusEditor(textarea) {
	if (textarea) {
		textarea.focus();
	}
}

// Enables Tab/Shift+Tab to indent instead of moving focus out of the editor, and returns the
// disposable handler reference is not needed because Blazor disposes the element. Returns true.
export function enableTabIndent(textarea, indent) {
	if (!textarea) {
		return false;
	}

	textarea.addEventListener('keydown', function (e) {
		if (e.key !== 'Tab') {
			return;
		}

		e.preventDefault();
		const start = textarea.selectionStart;
		const end = textarea.selectionEnd;
		const value = textarea.value;
		textarea.value = value.substring(0, start) + indent + value.substring(end);
		const caret = start + indent.length;
		textarea.setSelectionRange(caret, caret);
		textarea.dispatchEvent(new Event('input', { bubbles: true }));
	});

	return true;
}
