using RazorJS.Compiler.TemplateBuilders;
using System;
using System.Web.Razor.Parser.SyntaxTree;

namespace RazorJS.Compiler.Translation
{
	public interface ISpanTranslator
	{
		Type[] SupportsCodeGenerators { get; }

		void Translate(Span span, ITemplateBuilder builder);
	}
}