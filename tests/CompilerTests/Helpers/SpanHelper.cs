using System.Web.Razor.Parser.SyntaxTree;
using System.Web.Razor.Text;
using System.Web.Razor.Tokenizer.Symbols;

namespace RazorJS.CompilerTests.Helpers
{
	public static class SpanHelper
	{
		public static Span BuildSpan(string content)
		{
			SpanBuilder builder = new SpanBuilder();
			builder.Accept(new HtmlSymbol(new SourceLocation(), content, HtmlSymbolType.Text));

			return builder.Build();
		}
	}
}