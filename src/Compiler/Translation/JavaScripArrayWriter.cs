using System;
using System.IO;

namespace RazorJS.Compiler.Translation
{
	public class JavaScripArrayWriter : IJavaScriptArrayWriter
	{
		private static readonly string _arrayName = "_tmpl";

		public static string ArrayName
		{
			get
			{
				return _arrayName;
			}
		}

		public void PushToJavaScriptArray(TextWriter writer, string content)
		{
			this.PushToJavaScriptArray(writer, content, false);
		}

		public void PushToJavaScriptArray(TextWriter writer, string content, bool quoted)
		{
			if (String.IsNullOrEmpty(content))
			{
				return;
			}

			string quote = quoted ? "'" : String.Empty;

			writer.Write(ArrayName + ".push(" + quote);
			writer.Write(content);
			writer.Write(quote + ");");
		}
	}
}