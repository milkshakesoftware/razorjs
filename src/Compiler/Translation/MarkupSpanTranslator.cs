using System;
using System.Text;
using System.Web.Razor.Generator;
using System.Web.Razor.Parser.SyntaxTree;
using RazorJS.Compiler.TemplateBuilders;

namespace RazorJS.Compiler.Translation
{
	public class MarkupSpanTranslator : ISpanTranslator
	{
		public bool Match(Span span)
		{
			if (span == null)
			{
				return false;
			}

			return span.CodeGenerator.GetType() == typeof(MarkupCodeGenerator);
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

			var content = new StringBuilder(span.Content);
			content.Replace("\"", "\\\"");
			content.Replace("'", "\\'");
			content.Replace("\r", "\\r");
			content.Replace("\n", "\\n");

			templateBuilder.Write(content.ToString(), true);
		}
	}
}