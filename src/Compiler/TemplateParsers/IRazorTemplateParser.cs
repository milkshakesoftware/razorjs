using System.IO;
using System.Web.Razor;

namespace RazorJS.Compiler.TemplateParsers
{
	public interface IRazorTemplateParser
	{
		ParserResults ParseTemplate(TextReader template);
	}
}