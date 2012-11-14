using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RazorJS.Compiler.TemplateBuilders
{
	public class RazorJSTemplateBuilder : ITemplateBuilder
	{
		private static readonly string _arrayName = "_tmpl";

		private readonly IList<string> _templateCollection;

		private readonly IList<HelperFunction> _helperCollection;

		public RazorJSTemplateBuilder()
		{
			this._templateCollection = new List<string>();
			this._helperCollection = new List<HelperFunction>();
		}

		public RazorJSTemplateBuilder(IList<string> templateCollection, IList<HelperFunction> helperCollection)
		{
			this._templateCollection = templateCollection;
			this._helperCollection = helperCollection;
		}

		public void Write(string templateCode)
		{
			this.Write(templateCode, false);
		}

		public void Write(string templateCode, bool quoted)
		{
			if (String.IsNullOrEmpty(templateCode))
			{
				return;
			}

			string quote = quoted ? "'" : String.Empty;

			this._templateCollection.Add(String.Concat(_arrayName, ".push(", quote, templateCode, quote, ");"));
		}

		public void AddCodeBlock(string code)
		{
			if (String.IsNullOrWhiteSpace(code))
			{
				return;
			}

			this._templateCollection.Add(code);
		}

		public void AddHelperFunction(HelperFunction function)
		{
			if (function == null)
			{
				return;
			}

			this._helperCollection.Add(function);
		}

		public CompilerResult Build()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("function (Model) {");
			sb.AppendLine(String.Format("var {0} = [];", _arrayName));
			sb.AppendLine(String.Join(Environment.NewLine, this._templateCollection.ToArray()));
			sb.AppendLine(String.Format("return {0}.join('');", _arrayName));
			sb.Append("}");

			return new CompilerResult(sb.ToString());
		}
	}
}