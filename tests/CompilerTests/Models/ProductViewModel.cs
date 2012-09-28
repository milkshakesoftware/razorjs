using System;

namespace RazorJS.CompilerTests.Models
{
	public class ProductViewModel
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public string ShortDescription { get; set; }

		public string Description { get; set; }

		public decimal Price { get; set; }

		public decimal? DiscountedPrice { get; set; }

		public DateTime? DiscountPriceValidUntil { get; set; }
	}
}