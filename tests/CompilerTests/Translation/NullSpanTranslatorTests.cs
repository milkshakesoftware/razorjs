using System;
using System.Web.Razor.Generator;
using System.Web.Razor.Parser.SyntaxTree;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RazorJS.Compiler.TemplateBuilders;
using RazorJS.Compiler.Translation;
using RazorJS.CompilerTests.Helpers;

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

		[TestMethod]
		public void Match_GivenNullSpan_ReturnsFalse()
		{
			var sut = new NullSpanTranslator();

			var result = sut.Match(null);

			Assert.IsFalse(result);
		}

		[TestMethod]
		public void Match_GivenMarkupKindAndNullCodeGenerator_ReturnsTrue()
		{
			var span = new Span(new SpanBuilder() { Kind = SpanKind.Markup, CodeGenerator = null });

			var sut = new NullSpanTranslator();

			var result = sut.Match(span);

			Assert.IsTrue(result);
		}

		[TestMethod]
		public void Match_GivenMarkupCodeGeneratorSpan_ReturnsFalse()
		{
			var span = new Span(new SpanBuilder() { Kind = SpanKind.Markup, CodeGenerator = new MarkupCodeGenerator() });

			var sut = new NullSpanTranslator();

			var result = sut.Match(span);

			Assert.IsFalse(result);
		}

		[TestMethod]
		public void Match_GivenSpanKindComment_ReturnsFalse()
		{
			var span = new Span(new SpanBuilder() { Kind = SpanKind.Comment, CodeGenerator = null });

			var sut = new NullSpanTranslator();

			var result = sut.Match(span);

			Assert.IsFalse(result);
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