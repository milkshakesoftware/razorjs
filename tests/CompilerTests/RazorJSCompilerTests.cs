using System;
using System.Collections.Generic;
using System.Web.Razor;
using System.Web.Razor.Parser.SyntaxTree;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RazorJS.Compiler;
using RazorJS.Compiler.TemplateBuilders;
using RazorJS.Compiler.TemplateParsers;
using RazorJS.Compiler.Translation;
using RazorJS.CompilerTests.Helpers;

namespace RazorJS.CompilerTests
{
	[TestClass]
	public class RazorJSCompilerTests
	{
		private Mock<ITemplateParser> _templateParser;
		private Mock<ITemplateBuilder> _templateBuilder;
		private Mock<IDocumentTranslator> _documentTranslator;

		private ParserResults _errorParserResult = new ParserResults(BlockHelper.BuildEmptyBlock(), new List<RazorError>() { new RazorError("a", new System.Web.Razor.Text.SourceLocation()) });
		private ParserResults _successParserResult = new ParserResults(BlockHelper.BuildEmptyBlock(), null);

		[TestInitialize]
		public void TestInitialize()
		{
			this._templateParser = new Mock<ITemplateParser>();
			this._templateParser.Setup(t => t.ParseTemplate(It.IsAny<string>())).Returns(this._successParserResult);

			this._templateBuilder = new Mock<ITemplateBuilder>();
			this._documentTranslator = new Mock<IDocumentTranslator>();
		}

		[TestMethod, ExpectedException(typeof(ArgumentNullException))]
		public void Compile_GivenNullRazorTemplate_ThrowsArgumentNullException()
		{
			var sut = this.CreateCompiler();

			sut.Compile(null);
		}

		[TestMethod, ExpectedException(typeof(ArgumentException))]
		public void Compile_GivenEmptyRazorTemplate_ThrowsArgumentException()
		{
			var sut = this.CreateCompiler();

			sut.Compile(String.Empty);
		}

		[TestMethod, ExpectedException(typeof(ArgumentException))]
		public void Compile_GivenWhiteSpaceRazorTemplate_ThrowsArgumentException()
		{
			var sut = this.CreateCompiler();

			sut.Compile("  ");
		}

		[TestMethod]
		public void Compile_CallsTemplateParser()
		{
			string template = "a";

			var sut = this.CreateCompiler();

			sut.Compile(template);

			this._templateParser.Verify(t => t.ParseTemplate(template));
		}

		[TestMethod]
		public void Compile_TemplateParserReturnsErrors_ReturnsCompilerResult()
		{
			this._templateParser.Setup(t => t.ParseTemplate(It.IsAny<string>())).Returns(this._errorParserResult);
			var sut = this.CreateCompiler();

			var result = sut.Compile("a");

			Assert.IsInstanceOfType(result, typeof(CompilerResult));
		}

		[TestMethod]
		public void Compile_TemplateParserReturnsErrors_CompilerResultNotSuccessful()
		{
			this._templateParser.Setup(t => t.ParseTemplate(It.IsAny<string>())).Returns(this._errorParserResult);
			var sut = this.CreateCompiler();

			var result = sut.Compile("a");

			Assert.IsFalse(result.Success);
		}

		[TestMethod]
		public void Compile_TemplateParserReturnsErrors_ReturnsRazorErrors()
		{
			this._templateParser.Setup(t => t.ParseTemplate(It.IsAny<string>())).Returns(this._errorParserResult);
			var sut = this.CreateCompiler();

			var result = sut.Compile("a");

			Assert.AreEqual(this._errorParserResult.ParserErrors, result.ParserErrors);
		}

		[TestMethod]
		public void Compile_CallsDocumentTranslator()
		{
			Block document = BlockHelper.BuildEmptyBlock();
			this._templateParser.Setup(t => t.ParseTemplate(It.IsAny<string>())).Returns(new ParserResults(document, null));

			var sut = this.CreateCompiler();

			var result = sut.Compile("a");

			this._documentTranslator.Verify(d => d.Translate(document, this._templateBuilder.Object));
		}

		[TestMethod]
		public void Compile_Success_CallsBuildInTemplateBuilder()
		{
			var sut = this.CreateCompiler();

			var result = sut.Compile("a");

			this._templateBuilder.Verify(t => t.Build());
		}

		[TestMethod]
		public void Compile_Success_ReturnsCompilerResultFromTemplateBuilder()
		{
			CompilerResult expected = new CompilerResult("a");
			this._templateBuilder.Setup(t => t.Build()).Returns(expected);

			var sut = this.CreateCompiler();

			var result = sut.Compile("a");

			Assert.AreEqual(expected, result);
		}

		private RazorJSCompiler CreateCompiler()
		{
			return new RazorJSCompiler(this._templateParser.Object, this._templateBuilder.Object, this._documentTranslator.Object);
		}
	}
}