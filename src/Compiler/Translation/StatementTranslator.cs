using System;
using System.Linq;
using System.Web.Razor.Generator;
using System.Web.Razor.Parser.SyntaxTree;
using RazorJS.Compiler.TemplateBuilders;
using RazorJS.Compiler.Translation.CodeTranslation;

namespace RazorJS.Compiler.Translation
{
	public class StatementTranslator : ISpanTranslator
	{
		private readonly ICodeSpanTranslator[] _codeSpanTranslators;

		public StatementTranslator(params ICodeSpanTranslator[] codeSpanTranslators)
		{
			this._codeSpanTranslators = codeSpanTranslators;
		}

		public bool Match(Span span)
		{
			if (span == null)
			{
				return false;
			}

			return span.CodeGenerator.GetType() == typeof(StatementCodeGenerator);
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

			ICodeSpanTranslator matchedTranslator = this._codeSpanTranslators.FirstOrDefault(c => c.Match(span));

			if (matchedTranslator != null)
			{
				matchedTranslator.Translate(span, templateBuilder);
			}
		}
	}
}