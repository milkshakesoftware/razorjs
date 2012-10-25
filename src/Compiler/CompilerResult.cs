using System.Collections.Generic;
using System.Web.Razor.Parser.SyntaxTree;

namespace RazorJS.Compiler
{
	public class CompilerResult
	{
		public CompilerResult(string razorJsTemplate)
		{
			this.Success = true;
			this.RazorJSTemplate = razorJsTemplate;
			this.ParserErrors = new List<RazorError>();
		}

		public CompilerResult(IList<RazorError> parserErrors)
		{
			this.Success = false;
			this.ParserErrors = parserErrors;
		}


		public string RazorJSTemplate { get; private set; }

		public bool Success { get; private set; }

		public IList<RazorError> ParserErrors { get; private set; }
	}
}