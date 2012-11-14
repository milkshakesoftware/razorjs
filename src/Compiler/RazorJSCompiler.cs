using System;
using System.Web.Razor;
using RazorJS.Compiler.TemplateBuilders;
using RazorJS.Compiler.TemplateParsers;
using RazorJS.Compiler.Translation;

namespace RazorJS.Compiler
{
	public class RazorJSCompiler
	{
		private readonly ITemplateParser _templateParser;

		private readonly ITemplateBuilder _templateBuilder;

		private readonly IDocumentTranslator _documentTranslator;

		public RazorJSCompiler()
		{
			this._templateParser = new CSharpRazorTemplateParser();
			this._templateBuilder = new RazorJSTemplateBuilder();
			this._documentTranslator = new DocumentTranslator();
		}

		public RazorJSCompiler(ITemplateParser templateParser, ITemplateBuilder templateBuilder, IDocumentTranslator documentTranslator)
		{
			this._templateParser = templateParser;
			this._templateBuilder = templateBuilder;
			this._documentTranslator = documentTranslator;
		}

		public virtual CompilerResult Compile(string razorTemplate)
		{
			if (razorTemplate == null) throw new ArgumentNullException("razorTemplate");
			if (String.IsNullOrWhiteSpace(razorTemplate)) throw new ArgumentException("razorTemplate cannot be empty or white-space!", "razorTemplate");

			ParserResults parserResults = this._templateParser.ParseTemplate(razorTemplate);

			if (!parserResults.Success)
			{
				return new CompilerResult(parserResults.ParserErrors);
			}

			this._documentTranslator.Translate(parserResults.Document, this._templateBuilder);

			return this._templateBuilder.Build();
		}
	}
}