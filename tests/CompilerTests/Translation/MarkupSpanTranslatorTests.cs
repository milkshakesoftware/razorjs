using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RazorJS.Compiler.Translation;
using RazorJS.CompilerTests.Helpers;
using System;
using System.IO;
using System.Text;
using System.Web.Razor.Parser.SyntaxTree;

namespace RazorJS.CompilerTests.Translation
{
	[TestClass]
	public class MarkupSpanTranslatorTests
	{
		private Mock<IJavaScriptArrayWriter> _javaScriptArrayWriter;

		[TestInitialize]
		public void TestInitialize()
		{
			this._javaScriptArrayWriter = new Mock<IJavaScriptArrayWriter>();
		}

		[TestMethod, ExpectedException(typeof(ArgumentNullException))]
		public void Translate_GivenNullSpan_ThrowsArgumentNullException()
		{
			var sut = new MarkupSpanTranslator(this._javaScriptArrayWriter.Object);

			using (TextWriter writer = new StringWriter())
			{
				sut.Translate(null, writer);
			}
		}

		[TestMethod, ExpectedException(typeof(ArgumentNullException))]
		public void Translate_GivenNullTextWriter_ThrowsArgumentNullException()
		{
			var sut = new MarkupSpanTranslator(this._javaScriptArrayWriter.Object);

			sut.Translate(new Span(new SpanBuilder()), null);
		}

		[TestMethod]
		public void Translate_GivenText_CallsArrayWriterQuoted()
		{
			var sut = new MarkupSpanTranslator(this._javaScriptArrayWriter.Object);

			string input = "Model";
			Span span = SpanHelper.BuildSpan(input);

			StringBuilder sb = new StringBuilder();
			using (TextWriter writer = new StringWriter(sb))
			{
				sut.Translate(span, writer);

				this._javaScriptArrayWriter.Verify(j => j.PushToJavaScriptArray(writer, input, true));
			}
		}

		[TestMethod]
		public void Translate_GivenTextWithDoubleQuotes_CallsArrayWriterQuoted_DoubleQuotesEscaped()
		{
			var sut = new MarkupSpanTranslator(this._javaScriptArrayWriter.Object);

			string input = "Hello \"Martin\"!";
			Span span = SpanHelper.BuildSpan(input);

			StringBuilder sb = new StringBuilder();
			using (TextWriter writer = new StringWriter(sb))
			{
				sut.Translate(span, writer);

				this._javaScriptArrayWriter.Verify(j => j.PushToJavaScriptArray(writer, input.Replace("\"", "\\\""), true));
			}
		}

		[TestMethod]
		public void Translate_GivenTextWithSingleQuotes_CallsArrayWriterQuoted_SingleQuotesEscaped()
		{
			var sut = new MarkupSpanTranslator(this._javaScriptArrayWriter.Object);

			string input = "Hello 'Martin'!";
			Span span = SpanHelper.BuildSpan(input);

			StringBuilder sb = new StringBuilder();
			using (TextWriter writer = new StringWriter(sb))
			{
				sut.Translate(span, writer);

				this._javaScriptArrayWriter.Verify(j => j.PushToJavaScriptArray(writer, input.Replace("'", "\\'"), true));
			}
		}

		[TestMethod]
		public void Translate_GivenTextWithLineBreakBegin_CallsArrayWriterQuoted_LineBreakEscaped()
		{
			var sut = new MarkupSpanTranslator(this._javaScriptArrayWriter.Object);

			string input = "Hello \rMartin!";
			Span span = SpanHelper.BuildSpan(input);

			StringBuilder sb = new StringBuilder();
			using (TextWriter writer = new StringWriter(sb))
			{
				sut.Translate(span, writer);

				this._javaScriptArrayWriter.Verify(j => j.PushToJavaScriptArray(writer, input.Replace("\r", "\\r"), true));
			}
		}

		[TestMethod]
		public void Translate_GivenTextWithLineBreakEnd_CallsArrayWriterQuoted_LineBreakEscaped()
		{
			var sut = new MarkupSpanTranslator(this._javaScriptArrayWriter.Object);

			string input = "Hello \nMartin!";
			Span span = SpanHelper.BuildSpan(input);

			StringBuilder sb = new StringBuilder();
			using (TextWriter writer = new StringWriter(sb))
			{
				sut.Translate(span, writer);

				this._javaScriptArrayWriter.Verify(j => j.PushToJavaScriptArray(writer, input.Replace("\n", "\\n"), true));
			}
		}
	}
}