using System.IO;
using System.Web.Razor;

namespace RazorJS.Compiler.TemplateParsers
{
	public class CSharpRazorTemplateParser : ITemplateParser
	{
		public ParserResults ParseTemplate(string template)
		{
			RazorEngineHost host = new RazorEngineHost(new CSharpRazorCodeLanguage());
			RazorTemplateEngine engine = new RazorTemplateEngine(host);

			using (StringReader sr = new StringReader(template))
			{
				return engine.ParseTemplate(sr);
			}
		}
	}
}