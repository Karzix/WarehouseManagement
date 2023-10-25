using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WarehouseManagement.DAL.Migrations
{
    public partial class DB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SupplierId",
                table: "ProductRemaining",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ProductRemaining_SupplierId",
                table: "ProductRemaining",
                column: "SupplierId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductRemaining_Supplier_SupplierId",
                table: "ProductRemaining",
                column: "SupplierId",
                principalTable: "Supplier",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductRemaining_Supplier_SupplierId",
                table: "ProductRemaining");

            migrationBuilder.DropIndex(
                name: "IX_ProductRemaining_SupplierId",
                table: "ProductRemaining");

            migrationBuilder.DropColumn(
                name: "SupplierId",
                table: "ProductRemaining");
        }
    }
}
