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

		[TestMethod, Ignore]
		public void Basic_NoModelDefinition()
		{
			RunTestWithView("Basic_NoModelDefinition");
		}

		[TestMethod, Ignore]
		public void Basic_StronglyTyped()
		{
			RunTestWithView("Basic_StronglyTyped");
		}

		[TestMethod, Ignore]
		public void Basic_IfStatement()
		{
			RunTestWithView("Basic_IfStatement");
		}

		private void RunTestWithView(string viewName)
		{
			string viewTemplate = GetViewTemplate(viewName);

			Trace.WriteLine(viewTemplate);

			var sut = new RazorJSCompiler();
			var result = sut.Compile(viewTemplate);

			Trace.WriteLine(result.RazorJSTemplate);
		}

		public static string GetViewTemplate(string viewName)
		{
			DirectoryInfo binFolder = new DirectoryInfo(Environment.CurrentDirectory);
			string path = Path.Combine(binFolder.Parent.Parent.FullName, String.Format(@"Views\{0}.cshtml", viewName));

			if (File.Exists(path))
			{
				return File.ReadAllText(path);
			}

			throw new FileNotFoundException("A view with the specified viewName, could not be found!", path);
		}
	}
}