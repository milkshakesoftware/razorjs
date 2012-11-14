using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RazorJS.Compiler.TemplateBuilders;
using RazorJS.Compiler.Translation.CodeTranslation;
using RazorJS.CompilerTests.Helpers;

namespace RazorJS.CompilerTests.Translation.CodeTranslation
{
	[TestClass]
	public class EndCodeBlockTranslatorTests
	{
		[TestMethod]
		public void ImplementsInterface()
		{
			var sut = new EndCodeBlockTranslator();

			Assert.IsInstanceOfType(sut, typeof(ICodeSpanTranslator));
		}

		[TestMethod]
		public void SupportedType_IsEndCodeBlock()
		{
			var sut = new EndCodeBlockTranslator();

			Assert.AreEqual(StatementTypes.EndCodeBlock, sut.SupportedType);
		}

		[TestMethod]
		public void Match_GivenNullSpan_ReturnsFalse()
		{
			var sut = new EndCodeBlockTranslator();

			var result = sut.Match(null);

			Assert.IsFalse(result);
		}

		[TestMethod]
		public void Match_GivenEndCodeBlockContent_ReturnsTrue()
		{
			var span = SpanHelper.BuildSpan("}");

			var sut = new EndCodeBlockTranslator();

			var result = sut.Match(span);

			Assert.IsTrue(result);
		}

		[TestMethod]
		public void Match_GivenEndCodeBlockContentEndingInLineBreak_ReturnsTrue()
		{
			var span = SpanHelper.BuildSpan("}\r\n");

			var sut = new EndCodeBlockTranslator();

			var result = sut.Match(span);

			Assert.IsTrue(result);
		}

		[TestMethod]
		public void Match_GivenEndCodeBlockContentStartingWithTabIndent_ReturnsTrue()
		{
			var span = SpanHelper.BuildSpan("\t}");

			var sut = new EndCodeBlockTranslator();

			var result = sut.Match(span);

			Assert.IsTrue(result);
		}

		[TestMethod]
		public void Match_GivenContentNotEndCodeBlock_ReturnsFalse()
		{
			var span = SpanHelper.BuildSpan("@Html.HiddenFor(m => m.Id)");

			var sut = new EndCodeBlockTranslator();

			var result = sut.Match(span);

			Assert.IsFalse(result);
		}

		[TestMethod, ExpectedException(typeof(ArgumentNullException))]
		public void Translate_GivenNullCode_ThrowsArgumentNullException()
		{
			Mock<ITemplateBuilder> templateBuilder = new Mock<ITemplateBuilder>();

			var sut = new EndCodeBlockTranslator();

			sut.Translate(null, templateBuilder.Object);
		}

		[TestMethod, ExpectedException(typeof(ArgumentNullException))]
		public void Translate_GivenNullTemplateBuilder_ThrowsArgumentNullException()
		{
			var sut = new EndCodeBlockTranslator();

			sut.Translate("a", null);
		}

		[TestMethod]
		public void Translate_CallsTemplateBuilder()
		{
			Mock<ITemplateBuilder> templateBuilder = new Mock<ITemplateBuilder>();

			var sut = new EndCodeBlockTranslator();

			sut.Translate("}", templateBuilder.Object);

			templateBuilder.Verify(t => t.AddCodeBlock("}"));
		}

		[TestMethod]
		public void Translate_CleanCode_WhenAddingToTemplateBuilder()
		{
			Mock<ITemplateBuilder> templateBuilder = new Mock<ITemplateBuilder>();

			var sut = new EndCodeBlockTranslator();

			sut.Translate("\t}\r\n", templateBuilder.Object);

			templateBuilder.Verify(t => t.AddCodeBlock("}"));
		}
	}
}