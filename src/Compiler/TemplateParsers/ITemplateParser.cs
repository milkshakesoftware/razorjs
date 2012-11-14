using System.Web.Razor;

namespace RazorJS.Compiler.TemplateParsers
{
	public interface ITemplateParser
	{
		ParserResults ParseTemplate(string template);
	}
}