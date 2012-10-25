using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RazorJS.Compiler.TemplateBuilders;
using RazorJS.Compiler.Translation;
using System;
using System.Web.Razor.Parser.SyntaxTree;

namespace RazorJS.CompilerTests.Translation
{
	[TestClass]
	public class StatementTranslatorTests
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
		public void TestMethod1()
		{
		}
	}
}