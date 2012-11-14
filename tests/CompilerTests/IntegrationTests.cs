using System;
using System.Diagnostics;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RazorJS.Compiler;

namespace RazorJS.CompilerTests
{
	[TestClass]
	public class IntegrationTests
	{
		[TestInitialize]
		public void TestInitialize()
		{
			Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));
		}

		[TestMethod]
		public void Debugger()
		{
			string viewTemplate = GetViewTemplate("Basic_StronglyTyped");

			Trace.WriteLine(viewTemplate);

			var sut = new RazorJSCompiler();
			var result = sut.Compile(viewTemplate);

			Trace.WriteLine(result.RazorJSTemplate);
		}

		public static string GetViewTemplate(string viewName)
		{
			DirectoryInfo binFolder = new DirectoryInfo(Environment.CurrentDirectory);
			string path = Path.Combine(binFolder.FullName, String.Format(@"Views\{0}.cshtml", viewName));

			if (File.Exists(path))
			{
				return File.ReadAllText(path);
			}

			throw new FileNotFoundException("A view with the specified viewName, could not be found!", path);
		}
	}
}