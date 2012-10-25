using RazorJS.Compiler.TemplateBuilders;
using RazorJS.Compiler.TemplateParsers;
using RazorJS.Compiler.Translation;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Razor;
using System.Web.Razor.Parser.SyntaxTree;

namespace RazorJS.Compiler
{
	public class RazorJSCompiler
	{
		private readonly ITemplateParser _templateParser;

		private readonly ITemplateBuilder _templateBuilder;

		private List<ISpanTranslator> _translators;

		public RazorJSCompiler()
		{
			this._templateParser = new CSharpRazorTemplateParser();
			this.CreateInternalTranslators();
		}

		public RazorJSCompiler(ITemplateParser templateParser, ITemplateBuilder templateBuilder, params ISpanTranslator[] translators)
		{
			this._templateParser = templateParser;
			this._templateBuilder = templateBuilder;

			this.CreateInternalTranslators();

			if (translators != null && translators.Length > 0)
			{
				this._translators.AddRange(translators);
			}
		}

		public CompilerResult Compile(string razorTemplate)
		{
			using (StringReader sr = new StringReader(razorTemplate))
			{
				this.Compile(sr, this._templateBuilder);
			}

			return null;
		}

		public virtual void Compile(StringReader razorTemplate, ITemplateBuilder templateBuilder)
		{
			RazorEngineHost host = new RazorEngineHost(new CSharpRazorCodeLanguage());
			RazorTemplateEngine engine = new RazorTemplateEngine(host);

			ParserResults parserResults = engine.ParseTemplate(razorTemplate);

			if (!parserResults.Success)
			{
				//return new CompilerResult(parserResults.ParserErrors);
			}

			this.Render(parserResults.Document, templateBuilder);
		}

		private void Render(Block document, ITemplateBuilder templateBuilder)
		{
			templateBuilder.Write("function (Model) { ");
			templateBuilder.Write("var _tmpl = []; ");

			if (document != null)
			{
				this.RenderBlock(document, templateBuilder);
			}

			templateBuilder.Write(" return _tmpl.join(''); };");
		}

		private void RenderBlock(Block block, ITemplateBuilder templateBuilder)
		{
			foreach (SyntaxTreeNode child in block.Children)
			{
				if (child.IsBlock)
				{
					this.RenderBlock(child as Block, templateBuilder);
				}
				else
				{
					this.RenderSpan(child as Span, templateBuilder);
				}
			}
		}

		private void RenderSpan(Span span, ITemplateBuilder templateBuilder)
		{
			if (span == null)
			{
				return;
			}

			System.Diagnostics.Trace.WriteLine(span.CodeGenerator.GetType() + ": " + span.Content);

			ISpanTranslator renderer = this._translators.FirstOrDefault(b => b.SupportsCodeGenerators.Contains(span.CodeGenerator.GetType()));

			if (renderer != null)
			{
				renderer.Translate(span, templateBuilder);
			}
			else if (span.Kind == SpanKind.Markup)
			{
				new NullSpanTranslator().Translate(span, templateBuilder);
			}
		}

		private void CreateInternalTranslators()
		{
			this._translators = new List<ISpanTranslator>();
			this._translators.Add(new MarkupSpanTranslator());
			this._translators.Add(new NullSpanTranslator());
			this._translators.Add(new ExpressionTranslator());
			this._translators.Add(new StatementTranslator());
		}
	}
}