using System;
using System.IO;
using System.Web.Razor.Generator;
using System.Web.Razor.Parser.SyntaxTree;

namespace RazorJS.Compiler.SpanRenderers
{
	public class CodeSpanRenderer : ISpanRenderer
	{
		public Type[] SupportsCodeGenerators
		{
			get { return new Type[] { typeof(CSharpRazorCodeGenerator), typeof(VBRazorCodeGenerator) }; }
		}

		public void Render(Span span, TextWriter writer)
		{
		}
	}
}