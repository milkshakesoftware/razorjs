using System;
using System.IO;
using System.Text;
using System.Web.Razor.Generator;
using System.Web.Razor.Parser.SyntaxTree;

namespace RazorJS.Compiler.Translation
{
	public class MarkupSpanTranslator : ISpanTranslator
	{
		private readonly IJavaScriptArrayWriter _javaScriptArrayWriter;

		public MarkupSpanTranslator()
		{
			this._javaScriptArrayWriter = new JavaScripArrayWriter();
		}

		public MarkupSpanTranslator(IJavaScriptArrayWriter javaScriptArrayWriter)
		{
			this._javaScriptArrayWriter = javaScriptArrayWriter;
		}

		public Type[] SupportsCodeGenerators
		{
			get { return new Type[] { typeof(MarkupCodeGenerator) }; }
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

			var content = new StringBuilder(span.Content);
			content.Replace("\"", "\\\"");
			content.Replace("'", "\\'");
			content.Replace("\r", "\\r");
			content.Replace("\n", "\\n");

			this._javaScriptArrayWriter.PushToJavaScriptArray(writer, content.ToString(), true);
		}
	}
}