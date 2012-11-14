using System.Web.Razor.Parser.SyntaxTree;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RazorJS.Compiler.TemplateBuilders;
using RazorJS.Compiler.Translation;
using RazorJS.CompilerTests.Helpers;

namespace RazorJS.CompilerTests.Translation
{
	[TestClass]
	public class IgnoreSpanTranslatorTests
	{
		[TestMethod]
		public void ImplementsInterface()
		{
			var sut = new IgnoreSpanTranslator();

			Assert.IsInstanceOfType(sut, typeof(ISpanTranslator));
		}

		[TestMethod]
		public void Match_GivenNullSpan_ReturnsTrue()
		{
			var sut = new IgnoreSpanTranslator();

			var result = sut.Match(null);

			Assert.IsTrue(result);
		}

		[TestMethod]
		public void Match_GivenTransitionFromModelDeclaration_ReturnsTrue()
		{
			ModelDeclaration modelDeclaration = SpanHelper.BuildModelDeclaration();

			var sut = new IgnoreSpanTranslator();

			var result = sut.Match(modelDeclaration.Transition);

			Assert.IsTrue(result);
		}

		[TestMethod]
		public void Match_GivenModelKeywordFromModelDeclaration_ReturnsTrue()
		{
			ModelDeclaration modelDeclaration = SpanHelper.BuildModelDeclaration();

			var sut = new IgnoreSpanTranslator();

			var result = sut.Match(modelDeclaration.Model);

			Assert.IsTrue(result);
		}

		[TestMethod]
		public void Match_GivenModelTypeFromModelDeclaration_ReturnsTrue()
		{
			ModelDeclaration modelDeclaration = SpanHelper.BuildModelDeclaration();

			var sut = new IgnoreSpanTranslator();

			var result = sut.Match(modelDeclaration.ModelType);

			Assert.IsTrue(result);
		}

		[TestMethod]
		public void Match_GivenSpanNotFromModelDeclaration_ReturnsFalse()
		{
			Span span = SpanHelper.BuildSpan("a");

			var sut = new IgnoreSpanTranslator();

			var result = sut.Match(span);

			Assert.IsFalse(result);
		}

		[TestMethod]
		public void Translate_TemplateBuilder_Write_IsNeverCalled()
		{
			ITemplateBuilder templateBuilder = new FailingTemplateBuilder();

			var sut = new IgnoreSpanTranslator();

			sut.Translate(SpanHelper.BuildSpan("a"), templateBuilder);
		}
	}

	internal class FailingTemplateBuilder : ITemplateBuilder
	{
		public void Write(string templateCode)
		{
			throw new System.NotImplementedException();
		}

		public void Write(string templateCode, bool quoted)
		{
			throw new System.NotImplementedException();
		}

		public void AddCodeBlock(string code)
		{
			throw new System.NotImplementedException();
		}

		public void AddHelperFunction(Compiler.HelperFunction function)
		{
			throw new System.NotImplementedException();
		}

		public Compiler.CompilerResult Build()
		{
			throw new System.NotImplementedException();
		}
	}
}