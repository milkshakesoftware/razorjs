// No usings needed

namespace RazorJS.Compiler.TemplateBuilders
{
	public interface ITemplateBuilder
	{
		void Write(string templateCode);

		void Write(string templateCode, bool quoted);

		void AddHelperFunction(HelperFunction function);

		CompilerResult Build();
	}
}