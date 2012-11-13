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
	public class MarkupSpanTranslatorTests
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
			var sut = new MarkupSpanTranslator();

			var result = sut.Match(null);

			Assert.IsFalse(result);
		}

		[TestMethod]
		public void Match_GivenMarkupCodeGeneratorSpan_ReturnsTrue()
		{
			var span = new Span(new SpanBuilder() { CodeGenerator = new MarkupCodeGenerator() });

			var sut = new MarkupSpanTranslator();

			var result = sut.Match(span);

			Assert.IsTrue(result);
		}

		[TestMethod]
		public void Match_GivenExpressionCodeGeneratorSpan_ReturnsFalse()
		{
			var span = new Span(new SpanBuilder() { CodeGenerator = new ExpressionCodeGenerator() });

			var sut = new MarkupSpanTranslator();

			var result = sut.Match(span);

			Assert.IsFalse(result);
		}

		[TestMethod, ExpectedException(typeof(ArgumentNullException))]
		public void Translate_GivenNullSpan_ThrowsArgumentNullException()
		{
			var sut = new MarkupSpanTranslator();

			sut.Translate(null, this._templateBuilder.Object);
		}

		[TestMethod, ExpectedException(typeof(ArgumentNullException))]
		public void Translate_GivenNullTemplateBuilder_ThrowsArgumentNullException()
		{
			var sut = new MarkupSpanTranslator();

			sut.Translate(new Span(new SpanBuilder()), null);
		}

		[TestMethod]
		public void Translate_GivenText_CallsTemplateBuilderQuoted()
		{
			var sut = new MarkupSpanTranslator();

			string input = "Model";
			Span span = SpanHelper.BuildSpan(input);

			sut.Translate(span, this._templateBuilder.Object);

			this._templateBuilder.Verify(t => t.Write(input, true));
		}

		[TestMethod]
		public void Translate_GivenTextWithDoubleQuotes_CallsTemplateBuilderQuoted_DoubleQuotesEscaped()
		{
			var sut = new MarkupSpanTranslator();

			string input = "Hello \"Martin\"!";
			Span span = SpanHelper.BuildSpan(input);

			sut.Translate(span, this._templateBuilder.Object);

			this._templateBuilder.Verify(t => t.Write(input.Replace("\"", "\\\""), true));
		}

		[TestMethod]
		public void Translate_GivenTextWithSingleQuotes_CallsTemplateBuilderQuoted_SingleQuotesEscaped()
		{
			var sut = new MarkupSpanTranslator();

			string input = "Hello 'Martin'!";
			Span span = SpanHelper.BuildSpan(input);

			sut.Translate(span, this._templateBuilder.Object);

			this._templateBuilder.Verify(t => t.Write(input.Replace("'", "\\'"), true));
		}

		[TestMethod]
		public void Translate_GivenTextWithLineBreakBegin_CallsTemplateBuilderQuoted_LineBreakEscaped()
		{
			var sut = new MarkupSpanTranslator();

			string input = "Hello \rMartin!";
			Span span = SpanHelper.BuildSpan(input);

			sut.Translate(span, this._templateBuilder.Object);

			this._templateBuilder.Verify(t => t.Write(input.Replace("\r", "\\r"), true));
		}

		[TestMethod]
		public void Translate_GivenTextWithLineBreakEnd_CallsTemplateBuilderQuoted_LineBreakEscaped()
		{
			var sut = new MarkupSpanTranslator();

			string input = "Hello \nMartin!";
			Span span = SpanHelper.BuildSpan(input);

			sut.Translate(span, this._templateBuilder.Object);

			this._templateBuilder.Verify(t => t.Write(input.Replace("\n", "\\n"), true));
		}
	}
}