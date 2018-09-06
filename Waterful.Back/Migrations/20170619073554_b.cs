using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Waterful.Back.Migrations
{
    public partial class b : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Aftersales_Workers_WorkerId1",
                table: "Aftersales");

            migrationBuilder.DropIndex(
                name: "IX_Aftersales_WorkerId1",
                table: "Aftersales");

            migrationBuilder.DropColumn(
                name: "WorkerId1",
                table: "Aftersales");

            migrationBuilder.AlterColumn<int>(
                name: "WorkerId",
                table: "Aftersales",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ServiceTime",
                table: "Aftersales",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "OrderId",
                table: "Aftersales",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Aftersales_WorkerId",
                table: "Aftersales",
                column: "WorkerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Aftersales_Workers_WorkerId",
                table: "Aftersales",
                column: "WorkerId",
                principalTable: "Workers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Aftersales_Workers_WorkerId",
                table: "Aftersales");

            migrationBuilder.DropIndex(
                name: "IX_Aftersales_WorkerId",
                table: "Aftersales");

            migrationBuilder.AddColumn<int>(
                name: "WorkerId1",
                table: "Aftersales",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "WorkerId",
                table: "Aftersales",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "ServiceTime",
                table: "Aftersales",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<string>(
                name: "OrderId",
                table: "Aftersales",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.CreateIndex(
                name: "IX_Aftersales_WorkerId1",
                table: "Aftersales",
                column: "WorkerId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Aftersales_Workers_WorkerId1",
                table: "Aftersales",
                column: "WorkerId1",
                principalTable: "Workers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
