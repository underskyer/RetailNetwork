using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace KassaStoreDataBase.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Currencies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ShortName = table.Column<string>(type: "text", nullable: false),
                    FullName = table.Column<string>(type: "text", nullable: false),
                    ToRubleRate = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Goods",
                columns: table => new
                {
                    Code = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    PriceRub = table.Column<decimal>(type: "numeric", nullable: false),
                    Count = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Goods", x => x.Code);
                });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "FullName", "ShortName", "ToRubleRate" },
                values: new object[,]
                {
                    { 1, "Рубли", "RUB", 1m },
                    { 2, "Доллары", "USD", 71.9077m },
                    { 3, "Йены", "JPY", 0.447911m }
                });

            migrationBuilder.InsertData(
                table: "Goods",
                columns: new[] { "Code", "Count", "Name", "PriceRub" },
                values: new object[,]
                {
                    { 1, 1001, "Тапки", 50m },
                    { 2, 100, "Вертолёт", 5000000m },
                    { 3, 1000000, "Шприц", 5m },
                    { 4, 1001, "Плюмбус", 76m },
                    { 5, 1001, "Замок", 42000000m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Currencies");

            migrationBuilder.DropTable(
                name: "Goods");
        }
    }
}
