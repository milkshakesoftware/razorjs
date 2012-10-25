using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RazorJS.Compiler;
using RazorJS.Compiler.TemplateBuilders;
using RazorJS.Compiler.Translation;
using RazorJS.CompilerTests.Helpers;
using System;
using System.Web.Razor.Parser.SyntaxTree;

namespace RazorJS.CompilerTests.Translation
{
	[TestClass]
	public class NullSpanTranslatorTests
	{
		private Mock<ITemplateBuilder> _templateBuilder;

		[TestInitialize]
		public void TestInitialize()
		{
			this._templateBuilder = new Mock<ITemplateBuilder>();
		}

		[TestMethod, ExpectedException(typeof(ArgumentNullException))]
		public void Translate_GivenNullSpan_ThrowsArgumentNullException()
		{
			var sut = new NullSpanTranslator();

			sut.Translate(null, this._templateBuilder.Object);
		}

		[TestMethod, ExpectedException(typeof(ArgumentNullException))]
		public void Translate_GivenNullTemplateBuilder_ThrowsArgumentNullException()
		{
			var sut = new NullSpanTranslator();

			sut.Translate(new Span(new SpanBuilder()), null);
		}

		[TestMethod]
		public void Translate_GivenText_CallsTemplateBuilderQuoted()
		{
			var sut = new NullSpanTranslator();

			string input = "asas}}";
			Span span = SpanHelper.BuildSpan(input);

			sut.Translate(span, this._templateBuilder.Object);

			this._templateBuilder.Verify(t => t.Write(input, true));
		}

		[TestMethod]
		public void Translate_GivenAtSign_DoesNotCallTemplateBuilder()
		{
			var sut = new NullSpanTranslator();

			string input = "@";
			Span span = SpanHelper.BuildSpan(input);

			sut.Translate(span, this._templateBuilder.Object);

			this._templateBuilder.Verify(t => t.Write(It.IsAny<string>()), Times.Never());
			this._templateBuilder.Verify(t => t.Write(It.IsAny<string>(), It.IsAny<bool>()), Times.Never());
		}
	}
}