using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Tables ----------------------------------------------------

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductId = table.Column<int>(type    : "int", nullable: false).Annotation("SqlServer:Identity", "1, 1"),
                    Name      = table.Column<string>(type : "nvarchar(max)", nullable: false),
                    Category  = table.Column<string>(type : "nvarchar(max)", nullable: false),
                    Price     = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Stock     = table.Column<int>(type    : "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductId);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ProductId", "Category", "Name", "Price", "Stock" },
                values: new object[] { 1, "Electronic", "Lenovo Thinkpad T480s", 1500m, 10 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Tables ----------------------------------
            migrationBuilder.DropTable(name: "Products");
        }
    }
}
