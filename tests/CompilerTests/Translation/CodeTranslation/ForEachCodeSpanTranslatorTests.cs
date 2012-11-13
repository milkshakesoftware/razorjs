using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RazorJS.Compiler.TemplateBuilders;
using RazorJS.Compiler.Translation.CodeTranslation;
using RazorJS.CompilerTests.Helpers;

namespace RazorJS.CompilerTests.Translation.CodeTranslation
{
	[TestClass]
	public class ForEachCodeSpanTranslatorTests
	{
		[TestMethod]
		public void ImplementsInterface()
		{
			var sut = new ForEachCodeSpanTranslator();

			Assert.IsInstanceOfType(sut, typeof(ICodeSpanTranslator));
		}

		[TestMethod]
		public void SupportedType_IsForEach()
		{
			var sut = new ForEachCodeSpanTranslator();

			Assert.AreEqual(StatementTypes.Foreach, sut.SupportedType);
		}

		[TestMethod]
		public void Match_GivenNullSpan_ReturnsFalse()
		{
			var sut = new ForEachCodeSpanTranslator();

			var result = sut.Match(null);

			Assert.IsFalse(result);
		}

		[TestMethod]
		public void Match_GivenForEachContent_ReturnsTrue()
		{
			var span = SpanHelper.BuildSpan("@foreach (var item in Collection) {");

			var sut = new ForEachCodeSpanTranslator();

			var result = sut.Match(span);

			Assert.IsTrue(result);
		}

		[TestMethod]
		public void Match_GivenContentNotForEach_ReturnsFalse()
		{
			var span = SpanHelper.BuildSpan("@Html.HiddenFor(m => m.Id)");

			var sut = new ForEachCodeSpanTranslator();

			var result = sut.Match(span);

			Assert.IsFalse(result);
		}

		[TestMethod, ExpectedException(typeof(ArgumentNullException))]
		public void Translate_GivenNullSpan_ThrowsArgumentNullException()
		{
			Mock<ITemplateBuilder> templateBuilder = new Mock<ITemplateBuilder>();

			var sut = new ForEachCodeSpanTranslator();

			sut.Translate(null, templateBuilder.Object);
		}

		[TestMethod, ExpectedException(typeof(ArgumentNullException))]
		public void Translate_GivenNullTemplateBuilder_ThrowsArgumentNullException()
		{
			var sut = new ForEachCodeSpanTranslator();

			sut.Translate(SpanHelper.BuildSpan("a"), null);
		}

		[TestMethod]
		public void Translate_CallsTemplateBuilder()
		{
			Mock<ITemplateBuilder> templateBuilder = new Mock<ITemplateBuilder>();

			var sut = new ForEachCodeSpanTranslator();

			sut.Translate(SpanHelper.BuildSpan("@foreach (var item in Collection) {"), templateBuilder.Object);

			templateBuilder.Verify(t => t.Write("for(var __i=0; __i<Collection.length; __i++) { var item = Collection[__i]; "));
		}
	}
}