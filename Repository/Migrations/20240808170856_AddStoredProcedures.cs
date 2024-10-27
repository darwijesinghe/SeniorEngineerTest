using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddStoredProcedures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Procedures ----------------------------------------------------

            // procedure to get the avarage price per category
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE GetAveragePricePerCategory
                                AS
                                BEGIN
                                    SELECT Category as [Category], cast(AVG(Price) as decimal(18,2)) AS [AveragePrice]
                                    FROM Products
                                    GROUP BY Category
                                END");

            // procedure to get the category of highest stock value
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE GetHighestStockValueCategory
                                AS
                                BEGIN
                                    SELECT TOP 1 Category
                                    FROM Products
                                    GROUP BY Category
                                    ORDER BY SUM(Stock * Price) DESC
                                END");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Procedures -------------------------------------------------------------
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetAveragePricePerCategory");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetHighestStockValueCategory");
        }
    }
}
