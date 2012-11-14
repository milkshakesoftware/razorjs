using System.Web.Razor.Parser.SyntaxTree;
using RazorJS.Compiler.TemplateBuilders;

namespace RazorJS.Compiler.Translation
{
	public class IgnoreSpanTranslator : ISpanTranslator
	{
		public bool Match(Span span)
		{
			if (span == null)
			{
				return true;
			}

			if (IsPartOfModelDeclaration(span))
			{
				return true;
			}

			return false;
		}

		public void Translate(Span span, ITemplateBuilder builder)
		{
			// Ignore any content in the span
		}

		private bool IsPartOfModelDeclaration(Span span)
		{
			return IsTransitionFromModelDeclaration(span) || IsModelKeywordFromModelDeclaration(span) || IsModelTypeFromModelDelcaration(span);
		}

		private bool IsTransitionFromModelDeclaration(Span span)
		{
			if (span == null)
			{
				return false;
			}

			return span.Kind == SpanKind.Transition && span.Content.Equals("@") && IsModelKeywordFromModelDeclaration(span.Next);
		}

		private bool IsModelKeywordFromModelDeclaration(Span span)
		{
			if (span == null || span.Previous == null)
			{
				return false;
			}

			return span.Kind == SpanKind.Code && span.Content.Equals("model") && span.Previous.Kind == SpanKind.Transition;
		}

		private bool IsModelTypeFromModelDelcaration(Span span)
		{
			return IsModelKeywordFromModelDeclaration(span.Previous);
		}
	}
}