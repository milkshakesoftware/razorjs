using System.IO;
using System.Web.Razor;

namespace RazorJS.Compiler.TemplateParsers
{
	public interface ITemplateParser
	{
		ParserResults ParseTemplate(TextReader template);
	}
}