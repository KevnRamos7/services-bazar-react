using Dapper;
using Microsoft.AspNetCore.Mvc;
using serviciosAppBazar.data;
using serviciosAppBazar.models;

namespace serviciosAppBazar.controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class SalesController : ControllerBase
	{
	
		private readonly DataContext _context;
		
		public SalesController(DataContext context)
		{
			_context = context;
		}
		
		[HttpGet("items{query}")]
		public async Task<ActionResult<List<Products>>> GetItems(string query)
		{
			string sql;
			if(query == null || query == ".")
				sql = "SELECT * FROM products";
			else
				sql = "SELECT * FROM products WHERE title LIKE '%' + @query + '%' ";
			
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("query", query);
			
			try
			{
				using var connection = _context.createConnectionMainDatabase();
				var items = await connection.QueryAsync<Products>(sql, parameters);
				foreach (var item in items)
				{
					item.imagesList = item.images.Split(',').ToList();
				}
				var result = items.Select(item => new 
				{
					item.id,
					item.title,
					item.description,
					item.price,
					item.discountPercentage,
					item.rating,
					item.stock,
					item.brand,
					item.category,
					item.thumbnail,
					item.imagesList
				}).ToList();
				
				return Ok(result);
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}
		
		[HttpGet("items/{id}")]
		public async Task<ActionResult<Products>> GetItem(int id)
		{
			string sql = "SELECT * FROM products WHERE id = @id";
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("id", id);
			
			try
			{
				using var connection = _context.createConnectionMainDatabase();
				var item = await connection.QueryFirstOrDefaultAsync<Products>(sql, parameters);
				if(item == null)
					return NotFound();
				
				item.imagesList = item.images.Split(',').ToList();
				var result = new 
				{
					item.id,
					item.title,
					item.description,
					item.price,
					item.discountPercentage,
					item.rating,
					item.stock,
					item.brand,
					item.category,
					item.thumbnail,
					item.imagesList
				};
				
				return Ok(result);
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}
		
		[HttpPost("addSale")]
		public async Task<ActionResult> AddSale(List<NewSale> sale)
		{
			string sql = "INSERT INTO sales (fechaVenta, total) VALUES (@fechaVenta, @total); SELECT CAST(SCOPE_IDENTITY() as int)";
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("fechaVenta", DateTime.Now);
			parameters.Add("total", sale.Sum(item => item.total) );
			
			try
			{
				using var connection = _context.createConnectionMainDatabase();
				int idSales = await connection.ExecuteScalarAsync<int>(sql, parameters);
				
				foreach (var item in sale)
				{
					sql = "INSERT INTO salesItem (idSales, idProduct, quantity, price, discount, total) VALUES (@idSales, @idProduct, @quantity, @price, @discount, @total)";
					parameters = new DynamicParameters();
					parameters.Add("idSales", idSales);
					parameters.Add("idProduct", item.idProduct);
					parameters.Add("quantity", item.quantity);
					parameters.Add("price", item.price);
					parameters.Add("discount", item.discount);
					parameters.Add("total", item.total);
					
					await connection.ExecuteAsync(sql, parameters);
				}
				
				return Ok();
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}
		
		[HttpGet("sales")]
		public async Task<ActionResult<List<SalesModel>>> GetSales()
		{
			string sql = "SELECT * FROM sales";
			
			try
			{
				using var connection = _context.createConnectionMainDatabase();
				var sales = await connection.QueryAsync<SalesModel>(sql);
				foreach (var sale in sales)
				{
					sql = "SELECT * FROM salesItem WHERE idSales = @idSales";
					DynamicParameters parameters = new DynamicParameters();
					parameters.Add("idSales", sale.idSales);
					
					var items = await connection.QueryAsync<SalesItemModel>(sql, parameters);
					sale.items = items.ToList();
				}
				
				return Ok(sales);
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}
		
		
	}
	
}