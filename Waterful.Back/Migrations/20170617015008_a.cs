using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Waterful.Back.Migrations
{
    public partial class a : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerType",
                table: "Customers");

            migrationBuilder.CreateTable(
                name: "Captchas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    Code = table.Column<string>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    Num = table.Column<int>(nullable: false),
                    Phone = table.Column<string>(nullable: true),
                    UpdateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Captchas", x => x.Id);
                });

            migrationBuilder.AddColumn<bool>(
                name: "IsAngel",
                table: "Customers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPay",
                table: "Customers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "QrImg",
                table: "Customers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAngel",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "IsPay",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "QrImg",
                table: "Customers");

            migrationBuilder.DropTable(
                name: "Captchas");

            migrationBuilder.AddColumn<int>(
                name: "CustomerType",
                table: "Customers",
                nullable: false,
                defaultValue: 0);
        }
    }
}
