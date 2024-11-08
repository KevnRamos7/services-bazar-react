namespace serviciosAppBazar.models
{
	public class SalesItemModel
	{
		public int idSalesItem { get; set; }
		public int idSales { get; set; }
		public int idProduct { get; set; }	
		public int quantity { get; set; }
		public double price { get; set; }
		public double discount { get; set; }
		public double total { get; set; }
	}
}