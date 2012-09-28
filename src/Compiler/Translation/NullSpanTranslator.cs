using System;
using System.IO;
using System.Web.Razor.Generator;
using System.Web.Razor.Parser.SyntaxTree;

namespace RazorJS.Compiler.Translation
{
	public class NullSpanTranslator : ISpanTranslator
	{
		private readonly IJavaScriptArrayWriter _javaScriptArrayWriter;

		public NullSpanTranslator()
		{
			this._javaScriptArrayWriter = new JavaScripArrayWriter();
		}

		public NullSpanTranslator(IJavaScriptArrayWriter javaScriptArrayWriter)
		{
			this._javaScriptArrayWriter = javaScriptArrayWriter;
		}

		public Type[] SupportsCodeGenerators
		{
			get { return new Type[] { typeof(SpanCodeGenerator) }; }
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

			if (!String.Equals(span.Content, "@"))
			{
				this._javaScriptArrayWriter.PushToJavaScriptArray(writer, span.Content, true);
			}
		}
	}
}