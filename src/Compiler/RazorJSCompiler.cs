using RazorJS.Compiler.Translation;
using RazorJS.Compiler.TemplateParsers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Razor;
using System.Web.Razor.Parser.SyntaxTree;

namespace RazorJS.Compiler
{
	public class RazorJSCompiler
	{
		private List<ISpanTranslator> _translators;

		private IRazorTemplateParser _templateParser;

		public RazorJSCompiler()
		{
			this._templateParser = new CSharpRazorTemplateParser();
			this.CreateInternalTranslators();
		}

		public RazorJSCompiler(IRazorTemplateParser templateParser, params ISpanTranslator[] translators)
		{
			this._templateParser = templateParser;

			this.CreateInternalTranslators();

			if (translators != null && translators.Length > 0)
			{
				this._translators.AddRange(translators);
			}
		}

		public CompilerResults Compile(string razorTemplate)
		{
			using (StringWriter sw = new StringWriter())
			{
				using (StringReader sr = new StringReader(razorTemplate))
				{
					return this.Compile(sr, sw);
				}
			}
		}

		public virtual CompilerResults Compile(StringReader razorTemplate, StringWriter output)
		{
			RazorEngineHost host = new RazorEngineHost(new CSharpRazorCodeLanguage());
			RazorTemplateEngine engine = new RazorTemplateEngine(host);

			ParserResults parserResults = engine.ParseTemplate(razorTemplate);

			if (!parserResults.Success)
			{
				return new CompilerResults(parserResults.ParserErrors);
			}

			this.Render(parserResults.Document, output);

			return new CompilerResults(output.GetStringBuilder().ToString());
		}

		private void Render(Block document, TextWriter output)
		{
			output.Write("function (Model) { ");
			output.Write("var _buf = []; ");

			if (document != null)
			{
				this.RenderBlock(document, output);
			}

			output.Write(" return _buf.join(''); };");
		}

		private void RenderBlock(Block block, TextWriter output)
		{
			foreach (SyntaxTreeNode child in block.Children)
			{
				if (child.IsBlock)
				{
					this.RenderBlock(child as Block, output);
				}
				else
				{
					this.RenderSpan(child as Span, output);
				}
			}
		}

		private void RenderSpan(Span span, TextWriter output)
		{
			if (span == null)
			{
				return;
			}

			System.Diagnostics.Trace.WriteLine(span.CodeGenerator.GetType() + ": " + span.Content);

			ISpanTranslator renderer = this._translators.FirstOrDefault(b => b.SupportsCodeGenerators.Contains(span.CodeGenerator.GetType()));

			if (renderer != null)
			{
				renderer.Translate(span, output);
			}
			else if (span.Kind == SpanKind.Markup)
			{
				new NullSpanTranslator().Translate(span, output);
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