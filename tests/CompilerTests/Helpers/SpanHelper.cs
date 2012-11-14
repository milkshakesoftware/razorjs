using System.Collections.Generic;
using System.Linq;
using System.Web.Razor;
using System.Web.Razor.Parser.SyntaxTree;
using System.Web.Razor.Text;
using System.Web.Razor.Tokenizer.Symbols;
using RazorJS.Compiler.TemplateParsers;

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

		public static ModelDeclaration BuildModelDeclaration()
		{
			CSharpRazorTemplateParser templateParser = new CSharpRazorTemplateParser();
			string modelDeclarationTemplate = "@model ProductViewModel\r\n@using RazorJS.CompilerTests.Models";

			ParserResults document = templateParser.ParseTemplate(modelDeclarationTemplate);

			IEnumerable<Span> spans = document.Document.Flatten();

			return new ModelDeclaration
			{
				Transition = spans.ElementAt(1),
				Model = spans.ElementAt(2),
				ModelType = spans.ElementAt(3)
			};
		}
	}

	public sealed class ModelDeclaration
	{
		public Span Transition { get; set; }

		public Span Model { get; set; }

		public Span ModelType { get; set; }
	}
}