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
	public class ExpressionTranslatorTests
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
			var sut = new ExpressionTranslator(this._javaScriptArrayWriter.Object);

			using (TextWriter writer = new StringWriter())
			{
				sut.Translate(null, writer);
			}
		}

		[TestMethod, ExpectedException(typeof(ArgumentNullException))]
		public void Translate_GivenNullTextWriter_ThrowsArgumentNullException()
		{
			var sut = new ExpressionTranslator(this._javaScriptArrayWriter.Object);

			sut.Translate(new Span(new SpanBuilder()), null);
		}

		[TestMethod]
		public void Translate_GivenText_CallsArrayWriter()
		{
			var sut = new ExpressionTranslator(this._javaScriptArrayWriter.Object);

			string input = "Model.Property";
			Span span = SpanHelper.BuildSpan(input);

			StringBuilder sb = new StringBuilder();
			using (TextWriter writer = new StringWriter(sb))
			{
				sut.Translate(span, writer);

				this._javaScriptArrayWriter.Verify(j => j.PushToJavaScriptArray(writer, input));
			}
		}
	}
}