
using System.Collections.Generic;

namespace NoSqlWebApp.Models
{
	public class Cart : IModel
	{
		public long Id { get; set; }
		public long UserId { get; set; }
		public virtual User User { get; set; }
		public virtual IList<Product> Products { get; set; } = new List<Product>();
	}
}
