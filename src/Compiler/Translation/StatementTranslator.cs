using RazorJS.Compiler.Translation.CodeTranslation;
using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Razor.Generator;
using System.Web.Razor.Parser.SyntaxTree;
using System.Collections.Generic;
using RazorJS.Compiler.TemplateBuilders;

namespace RazorJS.Compiler.Translation
{
	public class StatementTranslator : ISpanTranslator
	{
		private readonly ICodeSpanTranslator[] _codeSpanTranslators;

		public StatementTranslator(params ICodeSpanTranslator[] codeSpanTranslators)
		{
			this._codeSpanTranslators = codeSpanTranslators;
		}

		public Type[] SupportsCodeGenerators
		{
			get { return new Type[] { typeof(StatementCodeGenerator) }; }
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
			matchedTranslator.Translate(span, templateBuilder);
		}
	}
}