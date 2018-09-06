using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Waterful.Back.Migrations
{
    public partial class updateaddress : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AreaId",
                table: "Addresss",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Display",
                table: "Addresss",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AreaId",
                table: "Addresss");

            migrationBuilder.DropColumn(
                name: "Display",
                table: "Addresss");
        }
    }
}
