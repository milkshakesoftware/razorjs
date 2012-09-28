using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using RazorJS.Compiler;
using System.Diagnostics;

namespace RazorJS.CompilerTests
{
	[TestClass]
	public class RazorJSCompilerTests
	{
		[TestMethod]
		public void TestMethod1()
		{
			string viewTemplate = this.GetViewTemplate("Basic_NoModelDefinition");

			var sut = new RazorJSCompiler();
			var result = sut.Compile(viewTemplate);

			Debug.Write(result.RazorJSTemplate);
		}

		private string GetViewTemplate(string viewName)
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