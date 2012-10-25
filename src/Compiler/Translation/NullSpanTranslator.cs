using RazorJS.Compiler.TemplateBuilders;
using System;
using System.IO;
using System.Web.Razor.Generator;
using System.Web.Razor.Parser.SyntaxTree;

namespace RazorJS.Compiler.Translation
{
	public class NullSpanTranslator : ISpanTranslator
	{
		public Type[] SupportsCodeGenerators
		{
			get { return new Type[] { typeof(SpanCodeGenerator) }; }
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