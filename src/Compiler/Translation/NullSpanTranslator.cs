using System;
using System.Web.Razor.Generator;
using System.Web.Razor.Parser.SyntaxTree;
using RazorJS.Compiler.TemplateBuilders;

namespace RazorJS.Compiler.Translation
{
	public class NullSpanTranslator : ISpanTranslator
	{
		public bool Match(Span span)
		{
			if (span == null)
			{
				return false;
			}

			return span.Kind == SpanKind.Markup && span.CodeGenerator == SpanCodeGenerator.Null;
		}

		public void Translate(Span span, ITemplateBuilder templateBuilder)
		{
			if (span == null)
			{
				throw new ArgumentNullException("span");
			}

			if (templateBuilder == null)
			{
				throw new ArgumentNullException("templateBuilder");
			}

			if (!String.Equals(span.Content, "@"))
			{
				templateBuilder.Write(span.Content, true);
			}
		}
	}
}