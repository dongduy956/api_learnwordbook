using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace API.REPO.Migrations
{
    public partial class addfieldcodeaccount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Accounts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreateAt",
                value: new DateTime(2023, 3, 1, 19, 54, 11, 699, DateTimeKind.Local).AddTicks(6795));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreateAt",
                value: new DateTime(2023, 3, 1, 19, 54, 11, 696, DateTimeKind.Local).AddTicks(8281));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "Accounts");

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreateAt",
                value: new DateTime(2023, 2, 28, 9, 31, 19, 867, DateTimeKind.Local).AddTicks(1380));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreateAt",
                value: new DateTime(2023, 2, 28, 9, 31, 19, 864, DateTimeKind.Local).AddTicks(8325));
        }
    }
}
