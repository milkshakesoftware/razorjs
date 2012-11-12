using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RazorJS.Compiler;
using RazorJS.Compiler.TemplateBuilders;
using RazorJS.Compiler.Translation;
using RazorJS.CompilerTests.Helpers;
using System;
using System.Web.Razor.Parser.SyntaxTree;
using System.Web.Razor.Generator;

namespace RazorJS.CompilerTests.Translation
{
	[TestClass]
	public class ExpressionTranslatorTests
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
			var sut = new ExpressionTranslator();

			var result = sut.Match(null);

			Assert.IsFalse(result);
		}

		[TestMethod]
		public void Match_GivenExpressionCodeGeneratorSpan_ReturnsTrue()
		{
			var span = new Span(new SpanBuilder() { CodeGenerator = new ExpressionCodeGenerator() });

			var sut = new ExpressionTranslator();

			var result = sut.Match(span);

			Assert.IsTrue(result);
		}

		[TestMethod]
		public void Match_GivenMarkupCodeGeneratorSpan_ReturnsFalse()
		{
			var span = new Span(new SpanBuilder() { CodeGenerator = new MarkupCodeGenerator() });

			var sut = new ExpressionTranslator();

			var result = sut.Match(span);

			Assert.IsFalse(result);
		}

		[TestMethod, ExpectedException(typeof(ArgumentNullException))]
		public void Translate_GivenNullSpan_ThrowsArgumentNullException()
		{
			var sut = new ExpressionTranslator();

			sut.Translate(null, this._templateBuilder.Object);
		}

		[TestMethod, ExpectedException(typeof(ArgumentNullException))]
		public void Translate_GivenNullTemplateBuilder_ThrowsArgumentNullException()
		{
			var sut = new ExpressionTranslator();

			sut.Translate(new Span(new SpanBuilder()), null);
		}

		[TestMethod]
		public void Translate_GivenText_CallsTemplateBuilder()
		{
			var sut = new ExpressionTranslator();

			string input = "Model.Property";
			Span span = SpanHelper.BuildSpan(input);

			sut.Translate(span, this._templateBuilder.Object);

			this._templateBuilder.Verify(t => t.Write(input));
		}
	}
}