using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Razor.Generator;
using System.Web.Razor.Parser.SyntaxTree;

namespace RazorJS.Compiler.Translation
{
	public class StatementTranslator : ISpanTranslator
	{
		private readonly IJavaScriptArrayWriter _javaScriptArrayWriter;

		public StatementTranslator()
		{
			this._javaScriptArrayWriter = new JavaScripArrayWriter();
		}

		public StatementTranslator(IJavaScriptArrayWriter javaScriptArrayWriter)
		{
			this._javaScriptArrayWriter = javaScriptArrayWriter;
		}

		public Type[] SupportsCodeGenerators
		{
			get { return new Type[] { typeof(StatementCodeGenerator) }; }
		}

		public void Translate(Span span, TextWriter writer)
		{
			if (span == null)
			{
				throw new ArgumentNullException("span");
			}

			if (writer == null)
			{
				throw new ArgumentNullException("writer");
			}

			string code = this.CleanConentForCode(span.Content);

			var @foreach = Regex.Match(code, @"foreach *\( *(?<Type>[^ ]*) (?<Variable>[^ ]*) in (?<Enumerator>[^ )]*)\) *{");

			if (!@foreach.Success)
			{
				writer.Write(code);

				return;
			}

			var groups = @foreach.Groups;

			writer.Write(String.Format("for(var __i=0; __i<{0}.length; __i++) {{ var {1} = {0}[__i]; ", groups["Enumerator"].Value, groups["Variable"].Value));
		}

		private string CleanConentForCode(string code)
		{
			var codeContent = new StringBuilder(code);
			codeContent.Replace("\r", String.Empty);
			codeContent.Replace("\n", String.Empty);

			return codeContent.ToString();
		}
	}
}