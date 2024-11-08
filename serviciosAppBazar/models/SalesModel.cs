namespace serviciosAppBazar.models
{
	public class SalesModel
	{
		public int idSales { get; set; }
		public DateTime fechaVenta { get; set; }
		public double total { get; set; }
		public List<SalesItemModel> items { get; set; }
	}
	
	
}