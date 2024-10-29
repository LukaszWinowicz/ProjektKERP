using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MassUpdatePurchase",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PurchaseOrder = table.Column<string>(type: "nchar(9)", fixedLength: true, maxLength: 9, nullable: false),
                    LineNumber = table.Column<int>(type: "int", nullable: false),
                    Sequence = table.Column<int>(type: "int", nullable: false),
                    ConfirmedReceiptDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ChangedReceiptDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AddedByUserId = table.Column<int>(type: "int", nullable: false),
                    IsGenerated = table.Column<bool>(type: "bit", nullable: false),
                    GeneratedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MassUpdatePurchase", x => x.Id);
                    table.CheckConstraint("CK_MassUpdatePurchase_AddedByUserId_Positive", "[AddedByUserId] > 0");
                    table.CheckConstraint("CK_MassUpdatePurchase_LineNumber_Positive", "[LineNumber] > 0");
                    table.CheckConstraint("CK_MassUpdatePurchase_Sequence_Positive", "[Sequence] > 0");
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MassUpdatePurchase");
        }
    }
}
