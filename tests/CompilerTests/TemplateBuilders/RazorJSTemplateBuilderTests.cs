using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RazorJS.Compiler;
using RazorJS.Compiler.TemplateBuilders;
using System;
using System.Collections.Generic;

namespace RazorJS.CompilerTests.TemplateBuilders
{
	[TestClass]
	public class RazorJSTemplateBuilderTests
	{
		private Mock<IList<string>> _templateCollection;
		private Mock<IList<HelperFunction>> _helperCollection;

		[TestInitialize]
		public void TestInitialize()
		{
			this._templateCollection = new Mock<IList<string>>();
			this._helperCollection = new Mock<IList<HelperFunction>>();
		}

		[TestMethod]
		public void ImplementsInterface()
		{
			var sut = new RazorJSTemplateBuilder(this._templateCollection.Object, this._helperCollection.Object);

			Assert.IsInstanceOfType(sut, typeof(ITemplateBuilder));
		}

		[TestMethod]
		public void Write_GivenNullTemplateCode_Ignores()
		{
			var sut = new RazorJSTemplateBuilder(this._templateCollection.Object, this._helperCollection.Object);

			sut.Write(null);

			this._templateCollection.Verify(t => t.Add(It.IsAny<string>()), Times.Never());
		}

		[TestMethod]
		public void Write_GivenEmptyTemplateCode_Ignores()
		{
			var sut = new RazorJSTemplateBuilder(this._templateCollection.Object, this._helperCollection.Object);

			sut.Write(String.Empty);

			this._templateCollection.Verify(t => t.Add(It.IsAny<string>()), Times.Never());
		}

		[TestMethod]
		public void Write_GivenWhiteSpaceTemplateCode_AddsToCollection()
		{
			string input = " ";

			var sut = new RazorJSTemplateBuilder(this._templateCollection.Object, this._helperCollection.Object);

			sut.Write(input);

			this._templateCollection.Verify(t => t.Add("_tmpl.push(" + input + ");"));
		}

		[TestMethod]
		public void Write_GivenTextInput_AddsToCollection()
		{
			string input = "sdfsdfdf";

			var sut = new RazorJSTemplateBuilder(this._templateCollection.Object, this._helperCollection.Object);

			sut.Write(input);

			this._templateCollection.Verify(t => t.Add("_tmpl.push(" + input + ");"));
		}

		[TestMethod]
		public void Write_Quoted_GivenNullTemplateCode_Ignores()
		{
			var sut = new RazorJSTemplateBuilder(this._templateCollection.Object, this._helperCollection.Object);

			sut.Write(null, true);

			this._templateCollection.Verify(t => t.Add(It.IsAny<string>()), Times.Never());
		}

		[TestMethod]
		public void Write_Quoted_GivenEmptyTemplateCode_Ignores()
		{
			var sut = new RazorJSTemplateBuilder(this._templateCollection.Object, this._helperCollection.Object);

			sut.Write(String.Empty, true);

			this._templateCollection.Verify(t => t.Add(It.IsAny<string>()), Times.Never());
		}

		[TestMethod]
		public void Write_Quoted_GivenWhiteSpaceTemplateCode_AddsToCollection()
		{
			string input = " ";

			var sut = new RazorJSTemplateBuilder(this._templateCollection.Object, this._helperCollection.Object);

			sut.Write(input, true);

			this._templateCollection.Verify(t => t.Add("_tmpl.push('" + input + "');"));
		}

		[TestMethod]
		public void Write_Quoted_GivenTextInput_AddsToCollection()
		{
			string input = "sdfsdfdf";

			var sut = new RazorJSTemplateBuilder(this._templateCollection.Object, this._helperCollection.Object);

			sut.Write(input, true);

			this._templateCollection.Verify(t => t.Add("_tmpl.push('" + input + "');"));
		}

		[TestMethod]
		public void AddHelperFunction_GivenNullFunction_Ignores()
		{
			var sut = new RazorJSTemplateBuilder(this._templateCollection.Object, this._helperCollection.Object);

			sut.AddHelperFunction(null);

			this._helperCollection.Verify(h => h.Add(It.IsAny<HelperFunction>()), Times.Never());
		}

		[TestMethod]
		public void AddHelperFunction_GivenFunction_AddsToCollection()
		{
			HelperFunction input = new HelperFunction("test", "alert('test');");

			var sut = new RazorJSTemplateBuilder(this._templateCollection.Object, this._helperCollection.Object);

			sut.AddHelperFunction(input);

			this._helperCollection.Verify(h => h.Add(input));
		}

		[TestMethod]
		public void Build_ReturnsCompilerResult()
		{
			var sut = new RazorJSTemplateBuilder(this._templateCollection.Object, this._helperCollection.Object);

			var result = sut.Build();

			Assert.IsInstanceOfType(result, typeof(CompilerResult));
		}

		[TestMethod]
		public void Build_ResultContainsTemplateCollection()
		{
			IList<string> templateCollection = new List<string> { "a", "b" };
			var sut = new RazorJSTemplateBuilder(templateCollection, this._helperCollection.Object);

			CompilerResult result = sut.Build();

			Assert.AreEqual("a\r\nb", result.RazorJSTemplate);
		}

		[TestMethod]
		public void Build_CompilerResutl_EmptyErrorsList()
		{
			var sut = new RazorJSTemplateBuilder(this._templateCollection.Object, this._helperCollection.Object);

			var result = sut.Build();

			Assert.AreEqual(0, result.ParserErrors.Count);
		}
	}
}