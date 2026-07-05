// Majorsoft Blazor RichText (WYSIWYG) editor JS interop.
// The editing surface is a contenteditable element. C# stays the source of truth for the *Markdown*
// value: this module loads Markdown-rendered HTML into the element, applies formatting commands to the
// live selection, and serializes the DOM back to Markdown so the bound value never exposes HTML.

export function setHtml(element, html) {
	if (element) {
		element.innerHTML = html ?? '';
	}
}

export function getMarkdown(element) {
	return element ? serialize(element) : '';
}

export function focusEditor(element) {
	if (element) {
		element.focus();
	}
}

// Wires up keyboard-shortcut gating and clipboard image paste. Called on render so the allowed set of
// native shortcuts (Ctrl+B/I/U) follows the toolbar configuration: when the Text style section is hidden
// the matching shortcut is suppressed so the feature is consistently unavailable. Listeners are attached
// once; the allowed flags are refreshed on every call.
export function configure(element, allowBold, allowItalic, allowUnderline) {
	if (!element) {
		return;
	}

	element._bmdeShortcuts = { bold: allowBold, italic: allowItalic, underline: allowUnderline };

	if (element._bmdeConfigured) {
		return;
	}
	element._bmdeConfigured = true;

	element.addEventListener('keydown', e => {
		if (e.altKey || !(e.ctrlKey || e.metaKey)) {
			return;
		}
		const flags = element._bmdeShortcuts || {};
		const key = e.key.toLowerCase();
		if ((key === 'b' && !flags.bold) || (key === 'i' && !flags.italic) || (key === 'u' && !flags.underline)) {
			e.preventDefault();
		}
	});

	element.addEventListener('paste', e => handleImagePaste(e));

	//Make links inside the editor clickable (open in a new tab) instead of only placing the caret.
	element.addEventListener('click', e => {
		const anchor = e.target.closest ? e.target.closest('a') : null;
		if (anchor && anchor.getAttribute('href')) {
			e.preventDefault();
			window.open(anchor.href, '_blank', 'noopener');
		}
	});
}

// Inserts an image straight from the clipboard (e.g. a screenshot) as an inline data URL.
function handleImagePaste(e) {
	const items = e.clipboardData && e.clipboardData.items;
	if (!items) {
		return;
	}

	for (const item of items) {
		if (item.type && item.type.indexOf('image') === 0) {
			const file = item.getAsFile();
			if (file) {
				e.preventDefault();
				const reader = new FileReader();
				reader.onload = () => document.execCommand('insertHTML', false, `<img src="${escapeAttr(reader.result)}" alt="image" />`);
				reader.readAsDataURL(file);
				return;
			}
		}
	}
}

// Applies a formatting command to the current selection and returns the resulting Markdown.
// Link, Image and Table are handled separately (via popovers) by the insertLink/insertImage/insertTable
// entry points below, so they are intentionally not in this switch.
export function command(element, name) {
	if (!element) {
		return null;
	}

	element.focus();
	switch (name) {
		case 'Bold': document.execCommand('bold'); break;
		case 'Italic': document.execCommand('italic'); break;
		case 'Underline': document.execCommand('underline'); break;
		case 'Strikethrough': document.execCommand('strikeThrough'); break;
		case 'Heading1': document.execCommand('formatBlock', false, '<h1>'); break;
		case 'Heading2': document.execCommand('formatBlock', false, '<h2>'); break;
		case 'Heading3': document.execCommand('formatBlock', false, '<h3>'); break;
		case 'BulletList': document.execCommand('insertUnorderedList'); break;
		case 'NumberedList': document.execCommand('insertOrderedList'); break;
		case 'IndentIncrease': document.execCommand('indent'); break;
		case 'IndentDecrease': document.execCommand('outdent'); break;
		case 'Quote': document.execCommand('formatBlock', false, '<blockquote>'); break;
		case 'CodeBlock': document.execCommand('formatBlock', false, '<pre>'); break;
		case 'HorizontalRule': document.execCommand('insertHorizontalRule'); break;
		case 'InlineCode': wrapInlineCode(); break;
		case 'TaskList': document.execCommand('insertHTML', false, '<ul class="bmde-tasklist"><li><input type="checkbox" /> task</li></ul>'); break;
	}

	return serialize(element);
}

function wrapInlineCode() {
	const sel = window.getSelection();
	if (!sel || sel.rangeCount === 0) {
		return;
	}

	const text = sel.toString();
	if (!text) {
		document.execCommand('insertHTML', false, '<code>code</code>');
		return;
	}

	const range = sel.getRangeAt(0);
	const code = document.createElement('code');
	code.textContent = text;
	range.deleteContents();
	range.insertNode(code);
	sel.removeAllRanges();
}

// ---- Selection-aware insertion (Link / Image / Table) -----------------------------------------
// Opening a popover moves focus out of the contenteditable, which collapses the selection. We snapshot
// the live range when the command is invoked and restore it right before inserting, so the new content
// lands where the caret was and any selected text is used (e.g. as the link label).

let savedRange = null;

// Stores the current selection (if it is inside the editor) and returns its text, used to pre-fill the
// link/image popovers with whatever the user had selected.
export function saveSelection(element) {
	const sel = window.getSelection();
	if (element && sel && sel.rangeCount > 0 && element.contains(sel.anchorNode)) {
		savedRange = sel.getRangeAt(0).cloneRange();
		return sel.toString();
	}

	savedRange = null;
	return '';
}

function restoreSelection(element) {
	element.focus();
	if (savedRange) {
		const sel = window.getSelection();
		sel.removeAllRanges();
		sel.addRange(savedRange);
		savedRange = null;
	}
}

export function insertLink(element, text, url) {
	if (!element) {
		return null;
	}

	restoreSelection(element);
	const sel = window.getSelection();
	const label = (text && text.length ? text : (sel ? sel.toString() : '')) || (url || 'https://');
	document.execCommand('insertHTML', false, `<a href="${escapeAttr(url || 'https://')}">${escapeHtml(label)}</a>`);
	return serialize(element);
}

export function insertImage(element, alt, src) {
	if (!element || !src) {
		return element ? serialize(element) : null;
	}

	restoreSelection(element);
	document.execCommand('insertHTML', false, `<img src="${escapeAttr(src)}" alt="${escapeAttr(alt || '')}" />`);
	return serialize(element);
}

export function insertTable(element, rows, cols) {
	if (!element || rows < 1 || cols < 1) {
		return element ? serialize(element) : null;
	}

	restoreSelection(element);

	let html = '<table><thead><tr>';
	for (let c = 0; c < cols; c++) {
		html += `<th>Header ${c + 1}</th>`;
	}
	html += '</tr></thead><tbody>';
	for (let r = 0; r < rows; r++) {
		html += '<tr>';
		for (let c = 0; c < cols; c++) {
			html += '<td>&nbsp;</td>';
		}
		html += '</tr>';
	}
	html += '</tbody></table><p><br /></p>';

	document.execCommand('insertHTML', false, html);
	return serialize(element);
}

function escapeHtml(value) {
	return (value || '').replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;');
}

function escapeAttr(value) {
	return escapeHtml(value).replace(/"/g, '&quot;');
}

// ---- DOM -> Markdown serialization ------------------------------------------------------------

function serialize(root) {
	const md = serializeBlocks(root).replace(/\n{3,}/g, '\n\n').trim();
	return md.length ? md + '\n' : '';
}

function serializeBlocks(parent) {
	const blocks = [];
	parent.childNodes.forEach(node => {
		const block = serializeBlock(node);
		if (block !== null && block.trim() !== '') {
			blocks.push(block);
		}
	});
	return blocks.join('\n\n');
}

function serializeBlock(node) {
	if (node.nodeType === Node.TEXT_NODE) {
		return node.textContent.trim();
	}
	if (node.nodeType !== Node.ELEMENT_NODE) {
		return '';
	}

	const tag = node.tagName.toLowerCase();
	switch (tag) {
		case 'h1': return '# ' + inline(node);
		case 'h2': return '## ' + inline(node);
		case 'h3': return '### ' + inline(node);
		case 'h4': return '#### ' + inline(node);
		case 'h5': return '##### ' + inline(node);
		case 'h6': return '###### ' + inline(node);
		case 'blockquote':
			return serializeBlocks(node).split('\n').map(l => '> ' + l).join('\n');
		case 'ul': return serializeList(node, false);
		case 'ol': return serializeList(node, true);
		case 'pre': {
			const code = node.querySelector('code') || node;
			return '```\n' + code.textContent.replace(/\n+$/, '') + '\n```';
		}
		case 'table': return serializeTable(node);
		case 'hr': return '---';
		case 'p':
		case 'div':
			return inline(node);
		default:
			return inline(node);
	}
}

function serializeList(listNode, ordered) {
	const lines = [];
	let index = 1;
	listNode.querySelectorAll(':scope > li').forEach(li => {
		const checkbox = li.querySelector(':scope > input[type=checkbox]');
		const text = inline(li).trim();
		if (checkbox) {
			lines.push(`- [${checkbox.checked ? 'x' : ' '}] ${text}`);
		} else if (ordered) {
			lines.push(`${index++}. ${text}`);
		} else {
			lines.push(`- ${text}`);
		}
	});
	return lines.join('\n');
}

// Serializes a DOM table to a GitHub-style pipe table so it round-trips through the Markdown value.
function serializeTable(table) {
	const headerRow = table.querySelector('thead tr') || table.querySelector('tr');
	if (!headerRow) {
		return '';
	}

	const headers = [];
	headerRow.querySelectorAll('th, td').forEach(cell => headers.push(cellText(cell)));
	if (headers.length === 0) {
		return '';
	}

	const lines = [
		'| ' + headers.join(' | ') + ' |',
		'| ' + headers.map(() => '---').join(' | ') + ' |',
	];

	const bodyRows = table.querySelectorAll('tbody tr');
	const rows = bodyRows.length ? bodyRows : table.querySelectorAll('tr');
	rows.forEach(row => {
		if (!bodyRows.length && row === headerRow) {
			return; //Header already emitted when the table has no explicit <tbody>.
		}
		const cells = [];
		row.querySelectorAll('td, th').forEach(cell => cells.push(cellText(cell)));
		while (cells.length < headers.length) {
			cells.push(' ');
		}
		lines.push('| ' + cells.slice(0, headers.length).join(' | ') + ' |');
	});

	return lines.join('\n');
}

// A cell's inline Markdown with pipes/newlines neutralized so they don't break the table grid.
function cellText(cell) {
	const text = inline(cell).replace(/\|/g, '\\|').replace(/\n+/g, ' ').trim();
	return text.length ? text : ' ';
}

function inline(node) {
	let result = '';
	node.childNodes.forEach(child => {
		result += serializeInline(child);
	});
	return result;
}

function serializeInline(node) {
	if (node.nodeType === Node.TEXT_NODE) {
		return node.textContent;
	}
	if (node.nodeType !== Node.ELEMENT_NODE) {
		return '';
	}

	const tag = node.tagName.toLowerCase();
	switch (tag) {
		case 'strong':
		case 'b': return `**${inline(node)}**`;
		case 'em':
		case 'i': return `*${inline(node)}*`;
		case 'u': return `<u>${inline(node)}</u>`;
		case 'del':
		case 's':
		case 'strike': return `~~${inline(node)}~~`;
		case 'code': return '`' + node.textContent + '`';
		case 'a': return `[${inline(node)}](${node.getAttribute('href') || ''})`;
		case 'img': return `![${node.getAttribute('alt') || ''}](${node.getAttribute('src') || ''})`;
		case 'br': return '  \n';
		case 'input': return ''; //task list checkbox, handled by serializeList
		default: return inline(node);
	}
}
