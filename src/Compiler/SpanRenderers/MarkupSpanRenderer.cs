using System;
using System.IO;
using System.Text;
using System.Web.Razor.Generator;
using System.Web.Razor.Parser.SyntaxTree;

namespace RazorJS.Compiler.SpanRenderers
{
	public class MarkupSpanRenderer : ISpanRenderer
	{
		public Type[] SupportsCodeGenerators
		{
			get { return new Type[] { typeof(MarkupCodeGenerator) }; }
		}

		public void Render(Span span, TextWriter writer)
		{
			var content = new StringBuilder(span.Content);
			content.Replace("\"", "\\\"");
			content.Replace("'", "\\'");
			content.Replace("\r", "\\r");
			content.Replace("\n", "\\n");

			writer.Write("_buf.push('");
			writer.Write(content.ToString());
			writer.Write("');");
		}
	}
}