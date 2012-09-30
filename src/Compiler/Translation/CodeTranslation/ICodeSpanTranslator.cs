﻿using System.IO;
using System.Web.Razor.Parser.SyntaxTree;

namespace RazorJS.Compiler.Translation.CodeTranslation
{
	public interface ICodeSpanTranslator
	{
		StatementTypes SupportedType { get; }

		bool Match(Span span);

		void Translate(Span span, TextWriter writer);
	}
}