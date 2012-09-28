using System;
using System.IO;
using System.Web.Razor.Generator;
using System.Web.Razor.Parser.SyntaxTree;

namespace RazorJS.Compiler.SpanRenderers
{
	public class NullSpanRenderer : ISpanRenderer
	{
		public Type[] SupportsCodeGenerators
		{
			get { return new Type[] { typeof(SpanCodeGenerator) }; }
		}

		public void Render(Span span, TextWriter writer)
		{
			if (!String.Equals(span.Content, "@"))
			{
				writer.Write("_buf.push('");
				writer.Write(span.Content);
				writer.Write("');");
			}
		}
	}
}