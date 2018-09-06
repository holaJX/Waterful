using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Waterful.Back.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Coupons",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    CouponNo = table.Column<string>(maxLength: 30, nullable: true),
                    CouponType = table.Column<int>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    CustomerId = table.Column<int>(nullable: false),
                    Discount = table.Column<int>(nullable: false),
                    ExpiryDate = table.Column<DateTime>(nullable: false),
                    FeelTime = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Remark = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    UpdateTime = table.Column<DateTime>(nullable: false),
                    Used = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coupons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CouponUses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    CouponId = table.Column<int>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    CustomerId = table.Column<int>(nullable: false),
                    OrderId = table.Column<int>(nullable: false),
                    Remark = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    UpdateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CouponUses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    CustomerType = table.Column<int>(nullable: false),
                    FullName = table.Column<string>(nullable: true),
                    Ico = table.Column<string>(nullable: true),
                    IntroducId = table.Column<int>(nullable: false),
                    Mobile = table.Column<string>(nullable: true),
                    NickName = table.Column<string>(nullable: true),
                    OpenId = table.Column<string>(nullable: true),
                    RegisterType = table.Column<int>(nullable: false),
                    Remark = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    UpdateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    Amount = table.Column<decimal>(type: "decimal(65, 2)", nullable: false),
                    Channel = table.Column<int>(nullable: false),
                    Close = table.Column<bool>(nullable: false),
                    CloseTime = table.Column<DateTime>(nullable: true),
                    CouponNo = table.Column<string>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    CustomerId = table.Column<int>(nullable: false),
                    DepositAmount = table.Column<decimal>(type: "decimal(65, 2)", nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "decimal(65, 2)", nullable: false),
                    FilterPrice = table.Column<decimal>(type: "decimal(65, 2)", nullable: false),
                    InstallAmount = table.Column<decimal>(type: "decimal(65, 2)", nullable: false),
                    Mobile = table.Column<string>(nullable: true),
                    Month = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    NextPayTime = table.Column<DateTime>(nullable: true),
                    OrderNo = table.Column<string>(nullable: true),
                    ParentId = table.Column<int>(nullable: false),
                    PayNotify = table.Column<string>(nullable: true),
                    PayTime = table.Column<DateTime>(nullable: true),
                    Remark = table.Column<string>(nullable: true),
                    ServiceNumber = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    Street = table.Column<string>(nullable: true),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true)
                        .Annotation("MySql:ValueGeneratedOnAddOrUpdate", true),
                    Total = table.Column<decimal>(type: "decimal(65, 2)", nullable: false),
                    UpdateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    Banner = table.Column<string>(nullable: true),
                    CategoryId = table.Column<int>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    DepositAmount = table.Column<decimal>(type: "decimal(65, 2)", nullable: false),
                    Description = table.Column<string>(nullable: true),
                    FilterPrice = table.Column<decimal>(type: "decimal(65, 2)", nullable: false),
                    ImageUrl = table.Column<string>(nullable: true),
                    InstallFee = table.Column<decimal>(type: "decimal(65, 2)", nullable: false),
                    Level = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    OriginalPrice = table.Column<decimal>(type: "decimal(65, 2)", nullable: false),
                    PaymentType = table.Column<int>(nullable: false),
                    Price = table.Column<decimal>(type: "decimal(65, 2)", nullable: false),
                    ProductNo = table.Column<string>(nullable: true),
                    Remark = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Storage = table.Column<int>(nullable: false),
                    Summary = table.Column<string>(nullable: true),
                    UpdateTime = table.Column<DateTime>(nullable: false),
                    VideoSrc = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    CreateUser = table.Column<int>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    LoginTime = table.Column<DateTime>(nullable: false),
                    Mobile = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Password = table.Column<string>(maxLength: 30, nullable: true),
                    Remark = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    UpdateTime = table.Column<DateTime>(nullable: false),
                    UserName = table.Column<string>(maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Workers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    Logo = table.Column<string>(nullable: true),
                    Mobile = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Remark = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    UpdateTime = table.Column<DateTime>(nullable: false),
                    WorkerNo = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Addresss",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    CusomerId = table.Column<int>(nullable: false),
                    Mobile = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Remark = table.Column<string>(nullable: true),
                    Sort = table.Column<long>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    Street = table.Column<string>(nullable: true),
                    UpdateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresss", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Addresss_Customers_CusomerId",
                        column: x => x.CusomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    Amount = table.Column<decimal>(type: "decimal(65, 2)", nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    DepositAmount = table.Column<decimal>(type: "decimal(65, 2)", nullable: false),
                    FilterNumber = table.Column<int>(nullable: false),
                    FilterPrice = table.Column<decimal>(type: "decimal(65, 2)", nullable: false),
                    InstallAmount = table.Column<decimal>(type: "decimal(65, 2)", nullable: false),
                    Name = table.Column<string>(nullable: true),
                    OrderId = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    ProductType = table.Column<int>(nullable: false),
                    Remark = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Aftersales",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    Content = table.Column<string>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    CustomerId = table.Column<int>(nullable: false),
                    Grade = table.Column<int>(nullable: false),
                    OrderId = table.Column<string>(nullable: true),
                    Remark = table.Column<string>(nullable: true),
                    ServiceTime = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    UpdateTime = table.Column<DateTime>(nullable: false),
                    WorkerId = table.Column<string>(nullable: true),
                    WorkerId1 = table.Column<int>(nullable: true),
                    isClear = table.Column<bool>(nullable: false),
                    isOnTime = table.Column<bool>(nullable: false),
                    isTidy = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Aftersales", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Aftersales_Workers_WorkerId1",
                        column: x => x.WorkerId1,
                        principalTable: "Workers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Addresss_CusomerId",
                table: "Addresss",
                column: "CusomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Aftersales_WorkerId1",
                table: "Aftersales",
                column: "WorkerId1");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_ProductId",
                table: "OrderItems",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Addresss");

            migrationBuilder.DropTable(
                name: "Aftersales");

            migrationBuilder.DropTable(
                name: "Coupons");

            migrationBuilder.DropTable(
                name: "CouponUses");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Workers");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
