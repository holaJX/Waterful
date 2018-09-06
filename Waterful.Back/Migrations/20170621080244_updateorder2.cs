using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Waterful.Back.Migrations
{
    public partial class updateorder2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Timestamp",
                table: "Orders",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true)
                .Annotation("MySql:ValueGeneratedOnAddOrUpdate", true)
                .OldAnnotation("MySql:ValueGeneratedOnAddOrUpdate", true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Timestamp",
                table: "Orders",
                nullable: true,
                oldClrType: typeof(DateTime))
                .Annotation("MySql:ValueGeneratedOnAddOrUpdate", true)
                .OldAnnotation("MySql:ValueGeneratedOnAddOrUpdate", true);
        }
    }
}
