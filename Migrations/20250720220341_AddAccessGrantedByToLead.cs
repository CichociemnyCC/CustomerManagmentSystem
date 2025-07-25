using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRM_Duo_Creative.Migrations
{
    /// <inheritdoc />
    public partial class AddAccessGrantedByToLead : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccessData",
                table: "Leads");

            migrationBuilder.DropColumn(
                name: "AdditionalNotes",
                table: "Leads");

            migrationBuilder.DropColumn(
                name: "MetaSettings",
                table: "Leads");

            migrationBuilder.DropColumn(
                name: "SocialLinks",
                table: "Leads");

            migrationBuilder.UpdateData(
                table: "Leads",
                keyColumn: "Services",
                keyValue: null,
                column: "Services",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "Services",
                table: "Leads",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Leads",
                keyColumn: "Packages",
                keyValue: null,
                column: "Packages",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "Packages",
                table: "Leads",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Leads",
                keyColumn: "MetaAccount",
                keyValue: null,
                column: "MetaAccount",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "MetaAccount",
                table: "Leads",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Leads",
                keyColumn: "LocalData",
                keyValue: null,
                column: "LocalData",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "LocalData",
                table: "Leads",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Leads",
                keyColumn: "ContractNumber",
                keyValue: null,
                column: "ContractNumber",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "ContractNumber",
                table: "Leads",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Leads",
                keyColumn: "ContactPerson",
                keyValue: null,
                column: "ContactPerson",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "ContactPerson",
                table: "Leads",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Leads",
                keyColumn: "ContactData",
                keyValue: null,
                column: "ContactData",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "ContactData",
                table: "Leads",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "AccessGrantedBy",
                table: "Leads",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "MetaAgreement",
                table: "Leads",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Leads",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "SocialWebGoogleDrive",
                table: "Leads",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccessGrantedBy",
                table: "Leads");

            migrationBuilder.DropColumn(
                name: "MetaAgreement",
                table: "Leads");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Leads");

            migrationBuilder.DropColumn(
                name: "SocialWebGoogleDrive",
                table: "Leads");

            migrationBuilder.AlterColumn<string>(
                name: "Services",
                table: "Leads",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Packages",
                table: "Leads",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "MetaAccount",
                table: "Leads",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "LocalData",
                table: "Leads",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "ContractNumber",
                table: "Leads",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "ContactPerson",
                table: "Leads",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "ContactData",
                table: "Leads",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "AccessData",
                table: "Leads",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "AdditionalNotes",
                table: "Leads",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "MetaSettings",
                table: "Leads",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "SocialLinks",
                table: "Leads",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
