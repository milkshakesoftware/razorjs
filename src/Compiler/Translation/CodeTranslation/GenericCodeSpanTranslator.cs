using System;
using System.Web.Razor.Parser.SyntaxTree;
using RazorJS.Compiler.TemplateBuilders;

namespace RazorJS.Compiler.Translation.CodeTranslation
{
	public class GenericCodeSpanTranslator : ICodeSpanTranslator
	{
		public StatementTypes SupportedType
		{
			get { return StatementTypes.GenericCodeStatement; }
		}

		public bool Match(Span span)
		{
			if (span == null)
			{
				return false;
			}

			return span.Kind == SpanKind.Code;
		}

		public void Translate(string code, ITemplateBuilder templateBuilder)
		{
			if (templateBuilder == null)
			{
				throw new ArgumentNullException("templateBuilder");
			}

			templateBuilder.AddCodeBlock(code);
		}
	}
}