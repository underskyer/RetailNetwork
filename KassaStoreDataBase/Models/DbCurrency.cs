using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KassaStoreDataBase;

public class DbCurrency
{
	/// <summary>
	/// артикул
	/// </summary>
	[Key]
	public int Id { get; set; }
	public string ShortName { get; set; } = default!;
	public string FullName { get; set; } = default!;
	public decimal ToRubleRate { get; set; }
}
