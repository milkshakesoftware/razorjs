using System;
using System.Web.Razor.Parser.SyntaxTree;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RazorJS.Compiler.TemplateBuilders;
using RazorJS.Compiler.Translation.CodeTranslation;
using RazorJS.CompilerTests.Helpers;

namespace RazorJS.CompilerTests.Translation.CodeTranslation
{
	[TestClass]
	public class GenericCodeSpanTranslatorTests
	{
		[TestMethod]
		public void ImplementsInterface()
		{
			var sut = new GenericCodeSpanTranslator();

			Assert.IsInstanceOfType(sut, typeof(ICodeSpanTranslator));
		}

		[TestMethod]
		public void SupportedType_ReturnsGenericCodeStatement()
		{
			var sut = new GenericCodeSpanTranslator();

			Assert.AreEqual(StatementTypes.GenericCodeStatement, sut.SupportedType);
		}

		[TestMethod]
		public void Match_GivenNullSpan_ReturnsFalse()
		{
			var sut = new GenericCodeSpanTranslator();

			var result = sut.Match(null);

			Assert.IsFalse(result);
		}

		[TestMethod]
		public void Match_GivenCodeSpan_ReturnsTrue()
		{
			Span codeSpan = SpanHelper.BuildSpan("a", SpanKind.Code);

			var sut = new GenericCodeSpanTranslator();

			var result = sut.Match(codeSpan);

			Assert.IsTrue(result);
		}

		[TestMethod, ExpectedException(typeof(ArgumentNullException))]
		public void Translate_GivenNullTemplateBuilder_ThrowsArgumentNullException()
		{
			var sut = new GenericCodeSpanTranslator();

			sut.Translate("a", null);
		}

		[TestMethod]
		public void Translate_CallsTemplateBuilder()
		{
			Mock<ITemplateBuilder> templateBuilder = new Mock<ITemplateBuilder>();
			string expected = "a";

			var sut = new GenericCodeSpanTranslator();

			sut.Translate(expected, templateBuilder.Object);

			templateBuilder.Verify(t => t.AddCodeBlock(expected));
		}
	}
}