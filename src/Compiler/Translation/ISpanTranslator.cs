using System.Web.Razor.Parser.SyntaxTree;
using RazorJS.Compiler.TemplateBuilders;

namespace RazorJS.Compiler.Translation
{
	public interface ISpanTranslator
	{
		bool Match(Span span);

		void Translate(Span span, ITemplateBuilder builder);
	}
}