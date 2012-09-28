using Microsoft.VisualStudio.TestTools.UnitTesting;
using RazorJS.Compiler.Translation;
using System;
using System.IO;
using System.Text;

namespace RazorJS.CompilerTests.Translation
{
	[TestClass]
	public class JavaScriptArrayWriterTests
	{
		[TestMethod]
		public void PushToJavaScriptArray_GivenNullContent_Ignores()
		{
			string initial = "a";
			StringBuilder sb = new StringBuilder(initial);

			var sut = new JavaScripArrayWriter();

			using (TextWriter writer = new StringWriter(sb))
			{
				sut.PushToJavaScriptArray(writer, null);
			}

			Assert.AreEqual(initial, sb.ToString());
		}

		[TestMethod]
		public void PushToJavaScriptArray_GivenEmptyContent_Ignores()
		{
			string initial = "a";
			StringBuilder sb = new StringBuilder(initial);

			var sut = new JavaScripArrayWriter();

			using (TextWriter writer = new StringWriter(sb))
			{
				sut.PushToJavaScriptArray(writer, String.Empty);
			}

			Assert.AreEqual(initial, sb.ToString());
		}

		[TestMethod]
		public void PushToJavaScriptArray_GivenWhiteSpaceContent_AddsToWriter()
		{
			string initial = "a";
			string input = " ";
			StringBuilder sb = new StringBuilder(initial);

			var sut = new JavaScripArrayWriter();

			using (TextWriter writer = new StringWriter(sb))
			{
				sut.PushToJavaScriptArray(writer, input);
			}

			Assert.AreEqual(initial + JavaScripArrayWriter.ArrayName + ".push(" + input + ");", sb.ToString());
		}

		[TestMethod]
		public void PushToJavaScriptArray_GivenTextInput_AddsToWriter()
		{
			string initial = "a";
			string input = "sdfsdfdf";
			StringBuilder sb = new StringBuilder(initial);

			var sut = new JavaScripArrayWriter();

			using (TextWriter writer = new StringWriter(sb))
			{
				sut.PushToJavaScriptArray(writer, input);
			}

			Assert.AreEqual(initial + JavaScripArrayWriter.ArrayName + ".push(" + input + ");", sb.ToString());
		}

		[TestMethod]
		public void PushToJavaScriptArray_Quoted_GivenNullContent_Ignores()
		{
			string initial = "a";
			StringBuilder sb = new StringBuilder(initial);

			var sut = new JavaScripArrayWriter();

			using (TextWriter writer = new StringWriter(sb))
			{
				sut.PushToJavaScriptArray(writer, null, true);
			}

			Assert.AreEqual(initial, sb.ToString());
		}

		[TestMethod]
		public void PushToJavaScriptArray_Quoted_GivenEmptyContent_Ignores()
		{
			string initial = "a";
			StringBuilder sb = new StringBuilder(initial);

			var sut = new JavaScripArrayWriter();

			using (TextWriter writer = new StringWriter(sb))
			{
				sut.PushToJavaScriptArray(writer, String.Empty, true);
			}

			Assert.AreEqual(initial, sb.ToString());
		}

		[TestMethod]
		public void PushToJavaScriptArray_Quoted_GivenWhiteSpaceContent_AddsToWriter()
		{
			string initial = "a";
			string input = " ";
			StringBuilder sb = new StringBuilder(initial);

			var sut = new JavaScripArrayWriter();

			using (TextWriter writer = new StringWriter(sb))
			{
				sut.PushToJavaScriptArray(writer, input, true);
			}

			Assert.AreEqual(initial + JavaScripArrayWriter.ArrayName + ".push('" + input + "');", sb.ToString());
		}

		[TestMethod]
		public void PushToJavaScriptArray_Quoted_GivenTextInput_AddsToWriter()
		{
			string initial = "a";
			string input = "sdfsdfdf";
			StringBuilder sb = new StringBuilder(initial);

			var sut = new JavaScripArrayWriter();

			using (TextWriter writer = new StringWriter(sb))
			{
				sut.PushToJavaScriptArray(writer, input, true);
			}

			Assert.AreEqual(initial + JavaScripArrayWriter.ArrayName + ".push('" + input + "');", sb.ToString());
		}
	}
}