using System;
using System.Text;
using System.Web.Razor.Parser.SyntaxTree;
using RazorJS.Compiler.TemplateBuilders;

namespace RazorJS.Compiler.Translation.CodeTranslation
{
	public class EndCodeBlockTranslator : ICodeSpanTranslator
	{
		public StatementTypes SupportedType
		{
			get { return StatementTypes.EndCodeBlock; }
		}

		public bool Match(Span span)
		{
			if (span == null)
			{
				return false;
			}

			return String.Equals(CleanContentForCode(span.Content), "}");
		}

		public void Translate(string code, ITemplateBuilder templateBuilder)
		{
			if (code == null)
			{
				throw new ArgumentNullException("code");
			}

			if (templateBuilder == null)
			{
				throw new ArgumentNullException("templateBuilder");
			}

			templateBuilder.AddCodeBlock(CleanContentForCode(code));
		}

		private static string CleanContentForCode(string code)
		{
			var codeContent = new StringBuilder(code);
			codeContent.Replace("\r", String.Empty);
			codeContent.Replace("\n", String.Empty);
			codeContent.Replace("\t", String.Empty);

			return codeContent.ToString();
		}
	}
}