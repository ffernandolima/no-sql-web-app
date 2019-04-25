
namespace NoSqlWebApp.Models
{
	public class Product : IModel
	{
		public long Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
	}
}
