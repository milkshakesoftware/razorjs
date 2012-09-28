using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Razor.Parser.SyntaxTree;

namespace RazorJS.Compiler
{
	public class CompilerResults
	{
		public CompilerResults(string razorJsTemplate)
		{
			this.Success = true;
			this.RazorJSTemplate = razorJsTemplate;
			this.ParserErrors = new List<RazorError>();
		}

		public CompilerResults(IList<RazorError> parserErrors)
		{
			this.Success = false;
			this.ParserErrors = parserErrors;
		}


		public string RazorJSTemplate { get; private set; }

		public bool Success { get; private set; }

		public IList<RazorError> ParserErrors { get; private set; }
	}
}