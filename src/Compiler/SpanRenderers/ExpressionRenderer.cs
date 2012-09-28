using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Razor.Generator;
using System.Web.Razor.Parser.SyntaxTree;

namespace RazorJS.Compiler.SpanRenderers
{
	public class ExpressionRenderer : ISpanRenderer
	{
		public Type[] SupportsCodeGenerators
		{
			get { return new Type[] { typeof(ExpressionCodeGenerator) }; }
		}

		public void Render(Span span, TextWriter writer)
		{
			writer.Write("_buf.push(");
			writer.Write(span.Content);
			writer.Write(");");
		}
	}
}
