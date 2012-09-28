using RazorJS.Compiler.SpanRenderers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Razor;
using System.Web.Razor.Parser.SyntaxTree;

namespace RazorJS.Compiler
{
	public class RazorJSCompiler
	{
		private List<ISpanRenderer> _spanRenderers;

		public RazorJSCompiler()
		{
			this.CreateInternalSpanRenderers();
		}

		public RazorJSCompiler(params ISpanRenderer[] blockRenderers)
		{
			this.CreateInternalSpanRenderers();
			this._spanRenderers.AddRange(blockRenderers);
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

			ISpanRenderer renderer = this._spanRenderers.FirstOrDefault(b => b.SupportsCodeGenerators.Contains(span.CodeGenerator.GetType()));

			if (renderer != null)
			{
				renderer.Render(span, output);
			}
			else if (span.Kind == SpanKind.Markup)
			{
				new NullSpanRenderer().Render(span, output);
			}
		}

		private void CreateInternalSpanRenderers()
		{
			this._spanRenderers = new List<ISpanRenderer>();
			this._spanRenderers.Add(new MarkupSpanRenderer());
			this._spanRenderers.Add(new NullSpanRenderer());
			this._spanRenderers.Add(new ExpressionRenderer());
			this._spanRenderers.Add(new StatementRenderer());
		}
	}
}