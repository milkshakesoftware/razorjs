using System.IO;

namespace RazorJS.Compiler.Translation
{
	public interface IJavaScriptArrayWriter
	{
		void PushToJavaScriptArray(TextWriter writer, string content);

		void PushToJavaScriptArray(TextWriter writer, string content, bool quoted);
	}
}