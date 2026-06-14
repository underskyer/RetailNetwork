using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KassaStoreDataBase;

public class DbGood
{
	/// <summary>
	/// артикул
	/// </summary>
	[Key]
	public int Code { get; set; }
	public string Name { get; set; } = default!;
	public decimal PriceRub { get; set; }
	public int Count  { get; set; }
}
