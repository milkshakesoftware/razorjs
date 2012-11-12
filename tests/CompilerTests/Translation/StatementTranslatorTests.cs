using System;
using System.Web.Razor.Generator;
using System.Web.Razor.Parser.SyntaxTree;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RazorJS.Compiler.TemplateBuilders;
using RazorJS.Compiler.Translation;
using RazorJS.Compiler.Translation.CodeTranslation;

namespace RazorJS.CompilerTests.Translation
{
	[TestClass]
	public class StatementTranslatorTests
	{
		private Mock<ITemplateBuilder> _templateBuilder;

		private Mock<ICodeSpanTranslator> _codeSpanTranslator;

		[TestInitialize]
		public void TestInitialize()
		{
			this._templateBuilder = new Mock<ITemplateBuilder>();
			this._codeSpanTranslator = new Mock<ICodeSpanTranslator>();
		}

		[TestMethod]
		public void Match_GivenNullSpan_ReturnsFalse()
		{
			var sut = new StatementTranslator();

			var result = sut.Match(null);

			Assert.IsFalse(result);
		}

		[TestMethod]
		public void Match_GivenStatementCodeGeneratorSpan_ReturnsTrue()
		{
			var span = new Span(new SpanBuilder() { CodeGenerator = new StatementCodeGenerator() });

			var sut = new StatementTranslator();

			var result = sut.Match(span);

			Assert.IsTrue(result);
		}

		[TestMethod]
		public void Match_GivenMarkupCodeGeneratorSpan_ReturnsTrue()
		{
			var span = new Span(new SpanBuilder() { CodeGenerator = new MarkupCodeGenerator() });

			var sut = new StatementTranslator();

			var result = sut.Match(span);

			Assert.IsFalse(result);
		}

		[TestMethod, ExpectedException(typeof(ArgumentNullException))]
		public void Translate_GivenNullSpan_ThrowsArgumentNullException()
		{
			var sut = new StatementTranslator();

			sut.Translate(null, this._templateBuilder.Object);
		}

		[TestMethod, ExpectedException(typeof(ArgumentNullException))]
		public void Translate_GivenNullTemplateBuilder_ThrowsArgumentNullException()
		{
			var sut = new StatementTranslator();

			sut.Translate(new Span(new SpanBuilder()), null);
		}

		[TestMethod]
		public void Translate_CallsMatchOnCodeSpanTranslator()
		{
			var span = new Span(new SpanBuilder());
			var sut = new StatementTranslator(this._codeSpanTranslator.Object);

			sut.Translate(span, this._templateBuilder.Object);

			this._codeSpanTranslator.Verify(c => c.Match(span));
		}

		[TestMethod]
		public void Translate_FoundMatchingCodeSpanTranslator_CallsTranslater()
		{
			this._codeSpanTranslator.Setup(c => c.Match(It.IsAny<Span>())).Returns(true);

			var span = new Span(new SpanBuilder());
			var sut = new StatementTranslator(this._codeSpanTranslator.Object);

			sut.Translate(span, this._templateBuilder.Object);

			this._codeSpanTranslator.Verify(c => c.Translate(span, this._templateBuilder.Object));
		}

		[TestMethod]
		public void Translate_MultipleMatchingCodeSpanTranslator_CallsFirstTranslater()
		{
			Mock<ICodeSpanTranslator> secondTranslator = new Mock<ICodeSpanTranslator>();
			secondTranslator.Setup(c => c.Match(It.IsAny<Span>())).Returns(true);
			this._codeSpanTranslator.Setup(c => c.Match(It.IsAny<Span>())).Returns(true);

			var span = new Span(new SpanBuilder());
			var sut = new StatementTranslator(this._codeSpanTranslator.Object, secondTranslator.Object);

			sut.Translate(span, this._templateBuilder.Object);

			this._codeSpanTranslator.Verify(c => c.Translate(span, this._templateBuilder.Object));
			secondTranslator.Verify(c => c.Translate(It.IsAny<Span>(), It.IsAny<ITemplateBuilder>()), Times.Never());
		}

		[TestMethod]
		public void Translate_NoMatchingCodeSpanTranslator_DoNotCallTranslater()
		{
			this._codeSpanTranslator.Setup(c => c.Match(It.IsAny<Span>())).Returns(false);

			var span = new Span(new SpanBuilder());
			var sut = new StatementTranslator(this._codeSpanTranslator.Object);

			sut.Translate(span, this._templateBuilder.Object);

			this._codeSpanTranslator.Verify(c => c.Translate(It.IsAny<Span>(), It.IsAny<ITemplateBuilder>()), Times.Never());
		}
	}
}