using System;
using System.Web.Razor.Parser.SyntaxTree;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RazorJS.Compiler.TemplateBuilders;
using RazorJS.Compiler.Translation;
using RazorJS.CompilerTests.Helpers;

namespace RazorJS.CompilerTests.Translation
{
	[TestClass]
	public class DocumentTranslatorTests
	{
		private Mock<ITemplateBuilder> _templateBuilder;
		private Mock<ISpanTranslator> _spanTranslator;

		[TestInitialize]
		public void TestInitialize()
		{
			this._templateBuilder = new Mock<ITemplateBuilder>();
			this._spanTranslator = new Mock<ISpanTranslator>();
			this._spanTranslator.Setup(s => s.Match(It.IsAny<Span>())).Returns(true);
		}

		[TestMethod]
		public void ImplementsInterface()
		{
			var sut = this.CreateSut();

			Assert.IsInstanceOfType(sut, typeof(IDocumentTranslator));
		}

		[TestMethod, ExpectedException(typeof(ArgumentNullException))]
		public void Translate_GivenNullDocument_ThrowsArgumentNullException()
		{
			var sut = this.CreateSut();

			sut.Translate(null, this._templateBuilder.Object);
		}

		[TestMethod]
		public void Translate_GivenBlockWithNoChildren_NeverCallsSpanTranslator()
		{
			var sut = this.CreateSut();

			sut.Translate(BlockHelper.BuildEmptyBlock(), this._templateBuilder.Object);

			this._spanTranslator.Verify(s => s.Translate(It.IsAny<Span>(), It.IsAny<ITemplateBuilder>()), Times.Never());
		}

		[TestMethod]
		public void Translate_GivenBlockWithOneChildSpan_CallsMatchOnSpanTranslator()
		{
			Span span = SpanHelper.BuildSpan("a");
			Block document = BlockHelper.BuildWithChildren(span);

			var sut = this.CreateSut();

			sut.Translate(document, this._templateBuilder.Object);

			this._spanTranslator.Verify(s => s.Match(span));
		}

		[TestMethod]
		public void Translate_FoundMatchingSpanTranslator_CallsTranslater()
		{
			this._spanTranslator.Setup(s => s.Match(It.IsAny<Span>())).Returns(true);

			Span span = SpanHelper.BuildSpan("a");
			Block document = BlockHelper.BuildWithChildren(span);

			var sut = this.CreateSut();

			sut.Translate(document, this._templateBuilder.Object);

			this._spanTranslator.Verify(s => s.Translate(span, this._templateBuilder.Object));
		}

		[TestMethod]
		public void Translate_MultipleMatchingCodeSpanTranslator_CallsFirstTranslater()
		{
			Mock<ISpanTranslator> secondTranslator = new Mock<ISpanTranslator>();
			secondTranslator.Setup(c => c.Match(It.IsAny<Span>())).Returns(true);
			this._spanTranslator.Setup(c => c.Match(It.IsAny<Span>())).Returns(true);

			Span span = SpanHelper.BuildSpan("a");
			Block document = BlockHelper.BuildWithChildren(span);

			var sut = new DocumentTranslator(this._spanTranslator.Object, secondTranslator.Object);

			sut.Translate(document, this._templateBuilder.Object);

			this._spanTranslator.Verify(c => c.Translate(span, this._templateBuilder.Object));
			secondTranslator.Verify(c => c.Translate(It.IsAny<Span>(), It.IsAny<ITemplateBuilder>()), Times.Never());
		}

		[TestMethod]
		public void Translate_NoMatchingCodeSpanTranslator_DoNotCallTranslater()
		{
			this._spanTranslator.Setup(s => s.Match(It.IsAny<Span>())).Returns(false);

			Block document = BlockHelper.BuildWithChildren(SpanHelper.BuildSpan("a"));

			var sut = this.CreateSut();

			sut.Translate(document, this._templateBuilder.Object);

			this._spanTranslator.Verify(c => c.Translate(It.IsAny<Span>(), It.IsAny<ITemplateBuilder>()), Times.Never());
		}

		private DocumentTranslator CreateSut()
		{
			return new DocumentTranslator(this._spanTranslator.Object);
		}
	}
}