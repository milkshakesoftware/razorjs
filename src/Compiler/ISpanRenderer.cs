using System;
using System.IO;
using System.Web.Razor.Parser.SyntaxTree;

namespace RazorJS.Compiler
{
	public interface ISpanRenderer
	{
		Type[] SupportsCodeGenerators { get; }

		void Render(Span span, TextWriter writer);
	}
}