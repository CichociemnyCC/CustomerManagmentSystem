using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRM_Duo_Creative.Migrations
{
    /// <inheritdoc />
    public partial class AddRealizationStatusRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClientId",
                table: "RealizationStatuses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_RealizationStatuses_ClientId",
                table: "RealizationStatuses",
                column: "ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_RealizationStatuses_Clients_ClientId",
                table: "RealizationStatuses",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RealizationStatuses_Clients_ClientId",
                table: "RealizationStatuses");

            migrationBuilder.DropIndex(
                name: "IX_RealizationStatuses_ClientId",
                table: "RealizationStatuses");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "RealizationStatuses");
        }
    }
}
