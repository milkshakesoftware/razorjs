using System;
using System.Text.RegularExpressions;
using System.Web.Razor.Parser.SyntaxTree;
using RazorJS.Compiler.TemplateBuilders;

namespace RazorJS.Compiler.Translation.CodeTranslation
{
	public class ForEachCodeSpanTranslator : ICodeSpanTranslator
	{
		private const string FOREACH_REGEX = @"foreach *\( *(?<Type>[^ ]*) (?<Variable>[^ ]*) in (?<Enumerator>[^ )]*)\) *{";

		public StatementTypes SupportedType
		{
			get { return StatementTypes.Foreach; }
		}

		public bool Match(Span span)
		{
			if (span == null)
			{
				return false;
			}

			Match match = Regex.Match(span.Content, FOREACH_REGEX);

			return match.Success;
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

			Match match = Regex.Match(code, FOREACH_REGEX);

			templateBuilder.Write(String.Format("for(var __i=0; __i<{0}.length; __i++) {{ var {1} = {0}[__i]; ", match.Groups["Enumerator"].Value, match.Groups["Variable"].Value));
		}
	}
}