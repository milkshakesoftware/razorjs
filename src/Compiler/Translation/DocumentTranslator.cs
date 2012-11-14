using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Razor.Parser.SyntaxTree;
using RazorJS.Compiler.TemplateBuilders;

namespace RazorJS.Compiler.Translation
{
	public class DocumentTranslator : IDocumentTranslator
	{
		private readonly List<ISpanTranslator> _translators;

		public DocumentTranslator()
		{
			this._translators = new List<ISpanTranslator>();
			this._translators.Add(new MarkupSpanTranslator());
			this._translators.Add(new NullSpanTranslator());
			this._translators.Add(new ExpressionTranslator());
			this._translators.Add(new StatementTranslator());
		}

		public DocumentTranslator(params ISpanTranslator[] spanTranslators)
		{
			this._translators = new List<ISpanTranslator>();

			if (spanTranslators != null && spanTranslators.Length > 0)
			{
				this._translators.AddRange(spanTranslators);
			}
		}

		public void Translate(Block document, ITemplateBuilder templateBuilder)
		{
			if (document == null)
			{
				throw new ArgumentNullException("document");
			}

			this.TranslateBlock(document, templateBuilder);
		}

		private void TranslateBlock(Block block, ITemplateBuilder templateBuilder)
		{
			foreach (SyntaxTreeNode child in block.Children)
			{
				if (child.IsBlock)
				{
					this.TranslateBlock(child as Block, templateBuilder);
				}
				else
				{
					this.TranslateSpan(child as Span, templateBuilder);
				}
			}
		}

		private void TranslateSpan(Span span, ITemplateBuilder templateBuilder)
		{
			/*System.Diagnostics.Trace.WriteLine(span.CodeGenerator.GetType() + ": " + span.Content);*/

			ISpanTranslator translator = this._translators.FirstOrDefault(t => t.Match(span));

			if (translator != null)
			{
				translator.Translate(span, templateBuilder);
			}
		}
	}
}