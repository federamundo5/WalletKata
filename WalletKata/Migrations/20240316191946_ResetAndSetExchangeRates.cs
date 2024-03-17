using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WalletKata.Migrations
{
    public partial class ResetAndSetExchangeRates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Currency",
                columns: table => new
                {
                    CurrencyId = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Code = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currency", x => x.CurrencyId);
                });

            migrationBuilder.CreateTable(
                name: "CurrencyWallet",
                columns: table => new
                {
                    CurrencyWalletId = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    WalletId = table.Column<long>(type: "INTEGER", nullable: false),
                    CurrencyId = table.Column<long>(type: "INTEGER", nullable: false),
                    Amount = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrencyWallet", x => x.CurrencyWalletId);
                });

            migrationBuilder.CreateTable(
                name: "ExchangeRate",
                columns: table => new
                {
                    ExchangeRateId = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SourceCurrencyCode = table.Column<string>(type: "TEXT", nullable: false),
                    TargetCurrencyCode = table.Column<string>(type: "TEXT", nullable: false),
                    Rate = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExchangeRate", x => x.ExchangeRateId);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Wallet",
                columns: table => new
                {
                    WalletId = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wallet", x => x.WalletId);
                });

            migrationBuilder.InsertData(
                table: "Currency",
                columns: new[] { "CurrencyId", "Code" },
                values: new object[] { 1L, "ARS" });

            migrationBuilder.InsertData(
                table: "Currency",
                columns: new[] { "CurrencyId", "Code" },
                values: new object[] { 2L, "EUR" });

            migrationBuilder.InsertData(
                table: "Currency",
                columns: new[] { "CurrencyId", "Code" },
                values: new object[] { 3L, "USD" });

            migrationBuilder.InsertData(
                table: "ExchangeRate",
                columns: new[] { "ExchangeRateId", "Rate", "SourceCurrencyCode", "TargetCurrencyCode" },
                values: new object[] { 1L, 0.000833m, "ARS", "EUR" });

            migrationBuilder.InsertData(
                table: "ExchangeRate",
                columns: new[] { "ExchangeRateId", "Rate", "SourceCurrencyCode", "TargetCurrencyCode" },
                values: new object[] { 2L, 0.000870m, "ARS", "USD" });

            migrationBuilder.InsertData(
                table: "ExchangeRate",
                columns: new[] { "ExchangeRateId", "Rate", "SourceCurrencyCode", "TargetCurrencyCode" },
                values: new object[] { 3L, 1.05m, "EUR", "USD" });

            migrationBuilder.InsertData(
                table: "ExchangeRate",
                columns: new[] { "ExchangeRateId", "Rate", "SourceCurrencyCode", "TargetCurrencyCode" },
                values: new object[] { 4L, 1200m, "EUR", "ARS" });

            migrationBuilder.InsertData(
                table: "ExchangeRate",
                columns: new[] { "ExchangeRateId", "Rate", "SourceCurrencyCode", "TargetCurrencyCode" },
                values: new object[] { 5L, 1150m, "USD", "ARS" });

            migrationBuilder.InsertData(
                table: "ExchangeRate",
                columns: new[] { "ExchangeRateId", "Rate", "SourceCurrencyCode", "TargetCurrencyCode" },
                values: new object[] { 6L, 0.952m, "USD", "EUR" });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "UserId", "Name" },
                values: new object[] { 1L, null });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "UserId", "Name" },
                values: new object[] { 2L, null });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "UserId", "Name" },
                values: new object[] { 3L, null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Currency");

            migrationBuilder.DropTable(
                name: "CurrencyWallet");

            migrationBuilder.DropTable(
                name: "ExchangeRate");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Wallet");
        }
    }
}
