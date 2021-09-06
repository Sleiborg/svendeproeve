using Microsoft.EntityFrameworkCore.Migrations;

namespace svendeproeve_backend.Migrations
{
    public partial class init3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Brands_Products_productId",
                table: "Brands");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Brands_BrandId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_BrandId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Brands_productId",
                table: "Brands");

            migrationBuilder.DropColumn(
                name: "BrandId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "productId",
                table: "Brands");

            migrationBuilder.CreateTable(
                name: "ProductBrandCollections",
                columns: table => new
                {
                    productId = table.Column<int>(nullable: false),
                    BrandId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductBrandCollections", x => new { x.productId, x.BrandId });
                    table.ForeignKey(
                        name: "FK_ProductBrandCollections_Brands_BrandId",
                        column: x => x.BrandId,
                        principalTable: "Brands",
                        principalColumn: "BrandId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductBrandCollections_Products_productId",
                        column: x => x.productId,
                        principalTable: "Products",
                        principalColumn: "productId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductBrandCollections_BrandId",
                table: "ProductBrandCollections",
                column: "BrandId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductBrandCollections");

            migrationBuilder.AddColumn<int>(
                name: "BrandId",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "productId",
                table: "Brands",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_BrandId",
                table: "Products",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_Brands_productId",
                table: "Brands",
                column: "productId");

            migrationBuilder.AddForeignKey(
                name: "FK_Brands_Products_productId",
                table: "Brands",
                column: "productId",
                principalTable: "Products",
                principalColumn: "productId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Brands_BrandId",
                table: "Products",
                column: "BrandId",
                principalTable: "Brands",
                principalColumn: "BrandId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
