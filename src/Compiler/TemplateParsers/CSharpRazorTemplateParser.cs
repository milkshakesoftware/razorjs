using System.IO;
using System.Web.Razor;

namespace RazorJS.Compiler.TemplateParsers
{
	public class CSharpRazorTemplateParser : IRazorTemplateParser
	{
		public ParserResults ParseTemplate(TextReader template)
		{
			RazorEngineHost host = new RazorEngineHost(new CSharpRazorCodeLanguage());
			RazorTemplateEngine engine = new RazorTemplateEngine(host);

			return engine.ParseTemplate(template);
		}
	}
}