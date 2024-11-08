namespace serviciosAppBazar.models
{
	
	public class NewSale
	{
		public int idProduct { get; set; }
		public int quantity { get; set; }
		public double price { get; set; }
		public double discount { get; set; }
		public double total { get; set; }
	}
	
}