using System.Web.Razor.Parser.SyntaxTree;

namespace RazorJS.CompilerTests.Helpers
{
	public static class BlockHelper
	{
		public static Block BuildEmptyBlock()
		{
			var builder = new BlockBuilder();
			builder.Name = "Test";
			builder.Type = BlockType.Comment;

			return builder.Build();
		}

		public static Block BuildWithChildren(params SyntaxTreeNode[] children)
		{
			var builder = new BlockBuilder();
			builder.Name = "Test";
			builder.Type = BlockType.Comment;

			foreach (var item in children)
			{
				builder.Children.Add(item);
			}

			return builder.Build();
		}
	}
}