// No usings needed

namespace RazorJS.Compiler
{
	public class HelperFunction
	{
		public HelperFunction(string name, string body)
		{
			this.Name = name;
			this.Body = body;
		}

		public string Name { get; private set; }

		public string Body { get; private set; }
	}
}