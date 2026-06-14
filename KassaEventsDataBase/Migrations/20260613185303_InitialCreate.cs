using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KassaEventsDataBase.Migrations;

/// <inheritdoc />
public partial class InitialCreate : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "kass_events",
            columns: table => new
            {
                Timestamp = table.Column<DateTime>(type: "DateTime", nullable: false),
                TerminalId = table.Column<string>(type: "String", nullable: false),
                Good = table.Column<string>(type: "String", nullable: false),
                Amount = table.Column<decimal>(type: "Decimal(18,2)", nullable: false),
                Metadata = table.Column<Dictionary<string, string>>(type: "Map(String, String)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_kass_events", x => new { x.Timestamp, x.TerminalId });
            })
            .Annotation("ClickHouse:Engine", "MergeTree")
            .Annotation("ClickHouse:OrderBy", new[] { "Timestamp", "TerminalId" });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "kass_events");
    }
}
