using System;
using System.IO;
using System.Web.Razor.Parser.SyntaxTree;

namespace RazorJS.Compiler.Translation
{
	public interface ISpanTranslator
	{
		Type[] SupportsCodeGenerators { get; }

		void Translate(Span span, TextWriter writer);
	}
}