using System.Text.Json.Serialization;

namespace serviciosAppBazar.models
{
	public class Products
	{
		
		public int id { get; set; }
		public string title { get; set; }
		public string description { get; set; }
		public double price { get; set; }
		public double discountPercentage { get; set; }
		public double rating { get; set; }
		public int stock { get; set; }
		public string brand { get; set; }
		public string category {get; set;}
		public string thumbnail {get; set;}
		public string images {get; set;}
		
		[JsonPropertyName("images")]
		public List<string> imagesList {get; set;}
		
	}
}