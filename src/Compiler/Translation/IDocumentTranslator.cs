using System.Web.Razor.Parser.SyntaxTree;
using RazorJS.Compiler.TemplateBuilders;

namespace RazorJS.Compiler.Translation
{
	public interface IDocumentTranslator
	{
		void Translate(Block document, ITemplateBuilder templateBuilder);
	}
}