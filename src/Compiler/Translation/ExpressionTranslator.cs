using RazorJS.Compiler.TemplateBuilders;
using System;
using System.Web.Razor.Generator;
using System.Web.Razor.Parser.SyntaxTree;

namespace RazorJS.Compiler.Translation
{
	public class ExpressionTranslator : ISpanTranslator
	{
		public Type[] SupportsCodeGenerators
		{
			get { return new Type[] { typeof(ExpressionCodeGenerator) }; }
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

			templateBuilder.Write(span.Content);
		}
	}
}