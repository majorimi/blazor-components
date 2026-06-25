using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Majorsoft.Blazor.Components.Inputs
{
	/// <summary>
	/// Lightweight, dependency-free Markdown to HTML converter used by <see cref="MarkdownEditor"/> for the
	/// live preview. Supports the most common Markdown features: ATX headings, bold, italic, strikethrough,
	/// inline code, fenced code blocks, blockquotes, ordered/unordered/task lists, links, images, pipe tables,
	/// horizontal rules and paragraphs.
	/// <para>
	/// For safety raw HTML in the source is escaped, except a small allow-list of inline tags
	/// (<c>u</c>, <c>mark</c>, <c>sub</c>, <c>sup</c>, <c>br</c>) emitted by the editor toolbar.
	/// </para>
	/// </summary>
	public static class MarkdownConverter
	{
		private static readonly Regex _heading = new(@"^(#{1,6})\s+(.*?)\s*#*\s*$", RegexOptions.Compiled);
		private static readonly Regex _horizontalRule = new(@"^\s*([-*_])(\s*\1){2,}\s*$", RegexOptions.Compiled);
		private static readonly Regex _unorderedItem = new(@"^\s*[-*+]\s+(.*)$", RegexOptions.Compiled);
		private static readonly Regex _orderedItem = new(@"^\s*\d+\.\s+(.*)$", RegexOptions.Compiled);
		private static readonly Regex _taskItem = new(@"^\s*[-*+]\s+\[([ xX])\]\s+(.*)$", RegexOptions.Compiled);
		private static readonly Regex _tableSeparator = new(@"^\s*\|?\s*:?-{1,}:?\s*(\|\s*:?-{1,}:?\s*)*\|?\s*$", RegexOptions.Compiled);
		private static readonly Regex _orderedPrefix = new(@"^\d+\.\s", RegexOptions.Compiled);
		private static readonly Regex _codeRestore = new(@"@@CODE(\d+)@@", RegexOptions.Compiled);

		/// <summary>
		/// Converts the given Markdown <paramref name="markdown"/> source into an HTML string.
		/// </summary>
		/// <param name="markdown">Markdown source text. <c>null</c> or empty returns an empty string.</param>
		/// <returns>The rendered HTML.</returns>
		public static string ToHtml(string? markdown)
		{
			if (string.IsNullOrEmpty(markdown))
			{
				return string.Empty;
			}

			var lines = markdown.Replace("\r\n", "\n").Replace("\r", "\n").Split('\n');
			var sb = new StringBuilder();

			for (var i = 0; i < lines.Length; i++)
			{
				var line = lines[i];

				//Fenced code block: ``` or ~~~ optionally followed by a language.
				var fence = GetFence(line);
				if (fence is not null)
				{
					var lang = line.Trim().Substring(3).Trim();
					var code = new StringBuilder();
					i++;
					while (i < lines.Length && GetFence(lines[i]) != fence)
					{
						code.Append(lines[i]).Append('\n');
						i++;
					}

					var cssClass = string.IsNullOrEmpty(lang) ? "" : $" class=\"language-{Escape(lang)}\"";
					sb.Append($"<pre><code{cssClass}>{Escape(code.ToString().TrimEnd('\n'))}</code></pre>\n");
					continue;
				}

				if (string.IsNullOrWhiteSpace(line))
				{
					continue;
				}

				if (_horizontalRule.IsMatch(line))
				{
					sb.Append("<hr />\n");
					continue;
				}

				var headingMatch = _heading.Match(line);
				if (headingMatch.Success)
				{
					var level = headingMatch.Groups[1].Value.Length;
					sb.Append($"<h{level}>{Inline(headingMatch.Groups[2].Value)}</h{level}>\n");
					continue;
				}

				//Blockquote: collect consecutive '>' lines and render their content recursively.
				if (line.TrimStart().StartsWith(">"))
				{
					var inner = new StringBuilder();
					while (i < lines.Length && lines[i].TrimStart().StartsWith(">"))
					{
						inner.Append(Regex.Replace(lines[i].TrimStart(), @"^>\s?", "")).Append('\n');
						i++;
					}
					i--;
					sb.Append($"<blockquote>\n{ToHtml(inner.ToString())}</blockquote>\n");
					continue;
				}

				//Table: header row containing '|' immediately followed by a separator row.
				if (line.Contains('|') && i + 1 < lines.Length && _tableSeparator.IsMatch(lines[i + 1]))
				{
					i = AppendTable(sb, lines, i);
					continue;
				}

				//Lists: gather consecutive items of the same kind.
				if (_taskItem.IsMatch(line) || _unorderedItem.IsMatch(line) || _orderedItem.IsMatch(line))
				{
					i = AppendList(sb, lines, i);
					continue;
				}

				//Paragraph: collect following non-blank lines that are not block starters.
				var paragraph = new StringBuilder();
				while (i < lines.Length && !string.IsNullOrWhiteSpace(lines[i]) && !IsBlockStart(lines[i], i, lines))
				{
					if (paragraph.Length > 0)
					{
						//Two trailing spaces force a hard line break, otherwise lines are soft-joined.
						paragraph.Append(lines[i - 1].EndsWith("  ") ? "<br />\n" : "\n");
					}
					paragraph.Append(lines[i].Trim());
					i++;
				}
				i--;
				sb.Append($"<p>{Inline(paragraph.ToString())}</p>\n");
			}

			return sb.ToString();
		}

		private static bool IsBlockStart(string line, int index, string[] lines)
		{
			return GetFence(line) is not null
				|| _horizontalRule.IsMatch(line)
				|| _heading.IsMatch(line)
				|| line.TrimStart().StartsWith(">")
				|| _taskItem.IsMatch(line)
				|| _unorderedItem.IsMatch(line)
				|| _orderedItem.IsMatch(line)
				|| (line.Contains('|') && index + 1 < lines.Length && _tableSeparator.IsMatch(lines[index + 1]));
		}

		private static string? GetFence(string line)
		{
			var trimmed = line.TrimStart();
			if (trimmed.StartsWith("```"))
			{
				return "```";
			}
			if (trimmed.StartsWith("~~~"))
			{
				return "~~~";
			}
			return null;
		}

		//A single parsed list item with its indentation depth, kind and content.
		private readonly record struct ListItem(int Indent, bool Ordered, bool IsTask, bool Checked, string Content);

		private static int AppendList(StringBuilder sb, string[] lines, int index)
		{
			//Collect all consecutive list-item lines (any indentation) of this list block.
			var items = new List<ListItem>();
			var i = index;
			while (i < lines.Length && !string.IsNullOrWhiteSpace(lines[i]) && TryParseListItem(lines[i], out var item))
			{
				items.Add(item);
				i++;
			}

			RenderListLevel(sb, items, 0, items[0].Indent);
			return i - 1;
		}

		//Renders items at the given indentation as one list, descending into deeper-indented items as nested lists.
		private static int RenderListLevel(StringBuilder sb, List<ListItem> items, int pos, int levelIndent)
		{
			var first = items[pos];
			sb.Append(first.IsTask ? "<ul class=\"bmde-tasklist\">\n" : first.Ordered ? "<ol>\n" : "<ul>\n");

			while (pos < items.Count && items[pos].Indent >= levelIndent)
			{
				if (items[pos].Indent > levelIndent)
				{
					break; //Deeper items are consumed by the nested call below, not here.
				}

				var item = items[pos];
				sb.Append("<li>");
				if (item.IsTask)
				{
					sb.Append($"<input type=\"checkbox\" disabled{(item.Checked ? " checked" : "")} /> ");
				}
				sb.Append(Inline(item.Content));
				pos++;

				//Nest any immediately following deeper-indented items inside this <li>.
				if (pos < items.Count && items[pos].Indent > levelIndent)
				{
					pos = RenderListLevel(sb, items, pos, items[pos].Indent);
				}
				sb.Append("</li>\n");
			}

			sb.Append(first.IsTask ? "</ul>\n" : first.Ordered ? "</ol>\n" : "</ul>\n");
			return pos;
		}

		private static bool TryParseListItem(string line, out ListItem item)
		{
			var indent = GetIndent(line);

			var task = _taskItem.Match(line);
			if (task.Success)
			{
				item = new ListItem(indent, false, true, task.Groups[1].Value is "x" or "X", task.Groups[2].Value);
				return true;
			}

			var ul = _unorderedItem.Match(line);
			if (ul.Success)
			{
				item = new ListItem(indent, false, false, false, ul.Groups[1].Value);
				return true;
			}

			var ol = _orderedItem.Match(line);
			if (ol.Success)
			{
				item = new ListItem(indent, true, false, false, ol.Groups[1].Value);
				return true;
			}

			item = default;
			return false;
		}

		private static int GetIndent(string line)
		{
			var indent = 0;
			foreach (var ch in line)
			{
				if (ch == ' ')
				{
					indent++;
				}
				else if (ch == '\t')
				{
					indent += 4;
				}
				else
				{
					break;
				}
			}
			return indent;
		}

		private static int AppendTable(StringBuilder sb, string[] lines, int index)
		{
			var headers = SplitRow(lines[index]);
			var aligns = ParseAligns(lines[index + 1]);

			sb.Append("<table>\n<thead>\n<tr>");
			for (var c = 0; c < headers.Count; c++)
			{
				sb.Append($"<th{Align(aligns, c)}>{Inline(headers[c])}</th>");
			}
			sb.Append("</tr>\n</thead>\n<tbody>\n");

			var row = index + 2;
			while (row < lines.Length && lines[row].Contains('|') && !string.IsNullOrWhiteSpace(lines[row]))
			{
				var cells = SplitRow(lines[row]);
				sb.Append("<tr>");
				for (var c = 0; c < headers.Count; c++)
				{
					var value = c < cells.Count ? cells[c] : "";
					sb.Append($"<td{Align(aligns, c)}>{Inline(value)}</td>");
				}
				sb.Append("</tr>\n");
				row++;
			}

			sb.Append("</tbody>\n</table>\n");
			return row - 1;
		}

		private static List<string> SplitRow(string line)
		{
			var trimmed = line.Trim();
			if (trimmed.StartsWith("|"))
			{
				trimmed = trimmed.Substring(1);
			}
			if (trimmed.EndsWith("|"))
			{
				trimmed = trimmed.Substring(0, trimmed.Length - 1);
			}

			var cells = new List<string>();
			foreach (var cell in trimmed.Split('|'))
			{
				cells.Add(cell.Trim());
			}
			return cells;
		}

		private static List<string> ParseAligns(string separator)
		{
			var aligns = new List<string>();
			foreach (var cell in SplitRow(separator))
			{
				var c = cell.Trim();
				var left = c.StartsWith(":");
				var right = c.EndsWith(":");
				aligns.Add(left && right ? "center" : right ? "right" : left ? "left" : "");
			}
			return aligns;
		}

		private static string Align(List<string> aligns, int column)
		{
			if (column < aligns.Count && !string.IsNullOrEmpty(aligns[column]))
			{
				return $" style=\"text-align:{aligns[column]}\"";
			}
			return "";
		}

		private static string Inline(string text)
		{
			//Protect inline code spans so their contents are not transformed. The @@CODEn@@ sentinel is
			//made of characters left untouched by HTML escaping and is extremely unlikely in real text.
			var codes = new List<string>();
			text = Regex.Replace(text, "`([^`]+)`", m =>
			{
				codes.Add(m.Groups[1].Value);
				return $"@@CODE{codes.Count - 1}@@";
			});

			text = Escape(text);
			text = RestoreAllowedTags(text);

			//Images before links (they share the [..](..) shape).
			text = Regex.Replace(text, @"!\[([^\]]*)\]\(([^)\s]+)(?:\s+&quot;([^&]*)&quot;)?\)",
				m => $"<img src=\"{AttrUrl(m.Groups[2].Value)}\" alt=\"{m.Groups[1].Value}\"{Title(m.Groups[3].Value)} />");

			text = Regex.Replace(text, @"\[([^\]]+)\]\(([^)\s]+)(?:\s+&quot;([^&]*)&quot;)?\)",
				m => $"<a href=\"{AttrUrl(m.Groups[2].Value)}\"{Title(m.Groups[3].Value)}>{m.Groups[1].Value}</a>");

			text = Regex.Replace(text, @"\*\*([^*]+)\*\*", "<strong>$1</strong>");
			text = Regex.Replace(text, @"__([^_]+)__", "<strong>$1</strong>");
			text = Regex.Replace(text, @"\*([^*]+)\*", "<em>$1</em>");
			text = Regex.Replace(text, @"(?<![A-Za-z0-9])_([^_]+)_(?![A-Za-z0-9])", "<em>$1</em>");
			text = Regex.Replace(text, @"~~([^~]+)~~", "<del>$1</del>");

			//Restore (escaped) code spans last.
			text = _codeRestore.Replace(text, m => $"<code>{Escape(codes[int.Parse(m.Groups[1].Value)])}</code>");

			return text;
		}

		private static string Title(string title) => string.IsNullOrEmpty(title) ? "" : $" title=\"{title}\"";

		private static string AttrUrl(string url) => url.Replace("&quot;", "%22").Replace("\"", "%22");

		private static string Escape(string text) => text
			.Replace("&", "&amp;")
			.Replace("<", "&lt;")
			.Replace(">", "&gt;")
			.Replace("\"", "&quot;");

		private static string RestoreAllowedTags(string text)
		{
			foreach (var tag in new[] { "u", "mark", "sub", "sup" })
			{
				text = text.Replace($"&lt;{tag}&gt;", $"<{tag}>").Replace($"&lt;/{tag}&gt;", $"</{tag}>");
			}
			text = Regex.Replace(text, @"&lt;br\s*/?&gt;", "<br />");
			return text;
		}
	}
}
