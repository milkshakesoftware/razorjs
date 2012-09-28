using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RazorJS.Compiler.Translation;
using System;
using System.IO;
using System.Web.Razor.Parser.SyntaxTree;

namespace RazorJS.CompilerTests.Translation
{
	[TestClass]
	public class StatementTranslatorTests
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
			var sut = new StatementTranslator(this._javaScriptArrayWriter.Object);

			using (TextWriter writer = new StringWriter())
			{
				sut.Translate(null, writer);
			}
		}

		[TestMethod, ExpectedException(typeof(ArgumentNullException))]
		public void Translate_GivenNullTextWriter_ThrowsArgumentNullException()
		{
			var sut = new StatementTranslator(this._javaScriptArrayWriter.Object);

			sut.Translate(new Span(new SpanBuilder()), null);
		}

		[TestMethod]
		public void TestMethod1()
		{
		}
	}
}