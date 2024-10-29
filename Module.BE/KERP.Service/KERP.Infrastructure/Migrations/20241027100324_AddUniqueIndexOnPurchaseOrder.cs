using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueIndexOnPurchaseOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PurchaseOrder",
                table: "MassUpdatePurchase",
                newName: "PurchaseOrderNumber");

            migrationBuilder.CreateTable(
                name: "PurchaseOrders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PurchaseOrderNumber = table.Column<string>(type: "nchar(9)", fixedLength: true, maxLength: 9, nullable: false),
                    LineNumber = table.Column<int>(type: "int", nullable: false),
                    Sequence = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseOrders", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_PurchaseOrderNumber_LineNumber_Sequence",
                table: "PurchaseOrders",
                columns: new[] { "PurchaseOrderNumber", "LineNumber", "Sequence" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PurchaseOrders");

            migrationBuilder.RenameColumn(
                name: "PurchaseOrderNumber",
                table: "MassUpdatePurchase",
                newName: "PurchaseOrder");
        }
    }
}
