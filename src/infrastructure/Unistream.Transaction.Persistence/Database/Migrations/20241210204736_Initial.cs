using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Unistream.Transaction.Persistence.Database.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "client_balance",
                columns: table => new
                {
                    client_id = table.Column<Guid>(type: "TEXT", nullable: false),
                    balanace = table.Column<decimal>(type: "TEXT", nullable: false),
                    update_date_time = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_client_balance", x => x.client_id);
                });

            migrationBuilder.CreateTable(
                name: "transaction",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    client_id = table.Column<Guid>(type: "TEXT", nullable: false),
                    date_time = table.Column<DateTime>(type: "TEXT", nullable: false),
                    amount = table.Column<decimal>(type: "TEXT", nullable: false),
                    current_balance = table.Column<decimal>(type: "TEXT", nullable: false),
                    updated_balance = table.Column<decimal>(type: "TEXT", nullable: false),
                    is_reverted = table.Column<bool>(type: "INTEGER", nullable: false),
                    reverted_date_time = table.Column<DateTime>(type: "TEXT", nullable: true),
                    sys_created = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_transaction", x => x.id);
                    table.ForeignKey(
                        name: "fk_transaction_client_balance_client_id",
                        column: x => x.client_id,
                        principalTable: "client_balance",
                        principalColumn: "client_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_transaction_client_id",
                table: "transaction",
                column: "client_id");

            migrationBuilder.CreateIndex(
                name: "ix_transaction_date_time",
                table: "transaction",
                column: "date_time");

            migrationBuilder.CreateIndex(
                name: "ix_transaction_sys_created",
                table: "transaction",
                column: "sys_created");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "transaction");

            migrationBuilder.DropTable(
                name: "client_balance");
        }
    }
}
