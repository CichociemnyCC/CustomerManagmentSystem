using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRM_Duo_Creative.Migrations
{
    /// <inheritdoc />
    public partial class AddMissingLeadFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AccessData",
                table: "Leads",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "AdditionalNotes",
                table: "Leads",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "ContactPerson",
                table: "Leads",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "LocalData",
                table: "Leads",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "MetaAccount",
                table: "Leads",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "MetaSettings",
                table: "Leads",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "SocialLinks",
                table: "Leads",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccessData",
                table: "Leads");

            migrationBuilder.DropColumn(
                name: "AdditionalNotes",
                table: "Leads");

            migrationBuilder.DropColumn(
                name: "ContactPerson",
                table: "Leads");

            migrationBuilder.DropColumn(
                name: "LocalData",
                table: "Leads");

            migrationBuilder.DropColumn(
                name: "MetaAccount",
                table: "Leads");

            migrationBuilder.DropColumn(
                name: "MetaSettings",
                table: "Leads");

            migrationBuilder.DropColumn(
                name: "SocialLinks",
                table: "Leads");
        }
    }
}
