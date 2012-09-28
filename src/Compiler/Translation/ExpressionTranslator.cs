using System;
using System.IO;
using System.Web.Razor.Generator;
using System.Web.Razor.Parser.SyntaxTree;

namespace RazorJS.Compiler.Translation
{
	public class ExpressionTranslator : ISpanTranslator
	{
		private readonly IJavaScriptArrayWriter _javaScriptArrayWriter;

		public ExpressionTranslator()
		{
			this._javaScriptArrayWriter = new JavaScripArrayWriter();
		}

		public ExpressionTranslator(IJavaScriptArrayWriter javaScriptArrayWriter)
		{
			this._javaScriptArrayWriter = javaScriptArrayWriter;
		}

		public Type[] SupportsCodeGenerators
		{
			get { return new Type[] { typeof(ExpressionCodeGenerator) }; }
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

			this._javaScriptArrayWriter.PushToJavaScriptArray(writer, span.Content);
		}
	}
}