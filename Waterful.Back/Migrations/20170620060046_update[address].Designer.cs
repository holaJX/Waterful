using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Waterful.Core;
using Waterful.Core.Enums;

namespace Waterful.Back.Migrations
{
    [DbContext(typeof(PomeloMySqlDbContext))]
    [Migration("20170620060046_update[address]")]
    partial class updateaddress
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2");

            modelBuilder.Entity("Waterful.Core.Models.Address", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AreaId");

                    b.Property<DateTime>("CreateTime");

                    b.Property<int>("CusomerId");

                    b.Property<string>("Display");

                    b.Property<string>("Mobile");

                    b.Property<string>("Name");

                    b.Property<string>("Remark");

                    b.Property<long>("Sort");

                    b.Property<int>("Status");

                    b.Property<string>("Street");

                    b.Property<DateTime>("UpdateTime");

                    b.HasKey("Id");

                    b.HasIndex("CusomerId");

                    b.ToTable("Addresss");
                });

            modelBuilder.Entity("Waterful.Core.Models.Aftersale", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Content");

                    b.Property<DateTime>("CreateTime");

                    b.Property<int>("CustomerId");

                    b.Property<int>("Grade");

                    b.Property<int>("OrderId");

                    b.Property<string>("Remark");

                    b.Property<DateTime>("ServiceTime");

                    b.Property<int>("Status");

                    b.Property<DateTime>("UpdateTime");

                    b.Property<int>("WorkerId");

                    b.Property<bool>("isClear");

                    b.Property<bool>("isOnTime");

                    b.Property<bool>("isTidy");

                    b.HasKey("Id");

                    b.HasIndex("WorkerId");

                    b.ToTable("Aftersales");
                });

            modelBuilder.Entity("Waterful.Core.Models.Captcha", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code");

                    b.Property<DateTime>("CreateTime");

                    b.Property<int>("Num");

                    b.Property<string>("Phone");

                    b.Property<DateTime>("UpdateTime");

                    b.HasKey("Id");

                    b.ToTable("Captchas");
                });

            modelBuilder.Entity("Waterful.Core.Models.Commission", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(65, 2)");

                    b.Property<DateTime>("CreateTime");

                    b.Property<int>("CustomerId");

                    b.Property<decimal>("OrderAmount");

                    b.Property<int>("OrderId");

                    b.Property<decimal>("Rate")
                        .HasColumnType("decimal(65, 2)");

                    b.Property<int>("Status");

                    b.Property<DateTime?>("UpdateTime");

                    b.HasKey("Id");

                    b.ToTable("Commissions");
                });

            modelBuilder.Entity("Waterful.Core.Models.Coupon", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CouponNo")
                        .HasMaxLength(30);

                    b.Property<int>("CouponType");

                    b.Property<DateTime>("CreateTime");

                    b.Property<int>("CustomerId");

                    b.Property<int>("Discount");

                    b.Property<DateTime>("ExpiryDate");

                    b.Property<int>("FeelTime");

                    b.Property<string>("Name");

                    b.Property<string>("Remark");

                    b.Property<int>("Status");

                    b.Property<int>("Type");

                    b.Property<DateTime>("UpdateTime");

                    b.Property<bool>("Used");

                    b.HasKey("Id");

                    b.ToTable("Coupons");
                });

            modelBuilder.Entity("Waterful.Core.Models.CouponUse", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CouponId");

                    b.Property<DateTime>("CreateTime");

                    b.Property<int>("CustomerId");

                    b.Property<int>("OrderId");

                    b.Property<string>("Remark");

                    b.Property<int>("Status");

                    b.Property<DateTime>("UpdateTime");

                    b.HasKey("Id");

                    b.ToTable("CouponUses");
                });

            modelBuilder.Entity("Waterful.Core.Models.Customer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreateTime");

                    b.Property<string>("FullName");

                    b.Property<string>("Ico");

                    b.Property<int>("IntroducId");

                    b.Property<bool>("IsAngel");

                    b.Property<bool>("IsPay");

                    b.Property<string>("Mobile");

                    b.Property<string>("NickName");

                    b.Property<string>("OpenId");

                    b.Property<string>("QrImg");

                    b.Property<int>("RegisterType");

                    b.Property<string>("Remark");

                    b.Property<int>("Status");

                    b.Property<DateTime>("UpdateTime");

                    b.HasKey("Id");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("Waterful.Core.Models.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(65, 2)");

                    b.Property<int>("Channel");

                    b.Property<bool>("Close");

                    b.Property<DateTime?>("CloseTime");

                    b.Property<string>("CouponNo");

                    b.Property<DateTime>("CreateTime");

                    b.Property<int>("CustomerId");

                    b.Property<decimal>("DepositAmount")
                        .HasColumnType("decimal(65, 2)");

                    b.Property<decimal>("DiscountAmount")
                        .HasColumnType("decimal(65, 2)");

                    b.Property<decimal>("FilterPrice")
                        .HasColumnType("decimal(65, 2)");

                    b.Property<decimal>("InstallAmount")
                        .HasColumnType("decimal(65, 2)");

                    b.Property<string>("Mobile");

                    b.Property<int>("Month");

                    b.Property<string>("Name");

                    b.Property<DateTime?>("NextPayTime");

                    b.Property<string>("OrderNo");

                    b.Property<int>("ParentId");

                    b.Property<string>("PayNotify");

                    b.Property<DateTime?>("PayTime");

                    b.Property<string>("Remark");

                    b.Property<int>("ServiceNumber");

                    b.Property<int>("Status");

                    b.Property<string>("Street");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<decimal>("Total")
                        .HasColumnType("decimal(65, 2)");

                    b.Property<DateTime>("UpdateTime");

                    b.HasKey("Id");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("Waterful.Core.Models.OrderItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(65, 2)");

                    b.Property<DateTime>("CreateTime");

                    b.Property<decimal>("DepositAmount")
                        .HasColumnType("decimal(65, 2)");

                    b.Property<int>("FilterNumber");

                    b.Property<decimal>("FilterPrice")
                        .HasColumnType("decimal(65, 2)");

                    b.Property<decimal>("InstallAmount")
                        .HasColumnType("decimal(65, 2)");

                    b.Property<string>("Name");

                    b.Property<int>("OrderId");

                    b.Property<int>("ProductId");

                    b.Property<int>("ProductType");

                    b.Property<string>("Remark");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.HasIndex("ProductId");

                    b.ToTable("OrderItems");
                });

            modelBuilder.Entity("Waterful.Core.Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Banner");

                    b.Property<int>("CategoryId");

                    b.Property<DateTime>("CreateTime");

                    b.Property<decimal>("DepositAmount")
                        .HasColumnType("decimal(65, 2)");

                    b.Property<string>("Description");

                    b.Property<decimal>("FilterPrice")
                        .HasColumnType("decimal(65, 2)");

                    b.Property<string>("ImageUrl");

                    b.Property<decimal>("InstallFee")
                        .HasColumnType("decimal(65, 2)");

                    b.Property<int>("Level");

                    b.Property<string>("Name");

                    b.Property<decimal>("OriginalPrice")
                        .HasColumnType("decimal(65, 2)");

                    b.Property<int>("PaymentType");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(65, 2)");

                    b.Property<string>("ProductNo");

                    b.Property<string>("Remark");

                    b.Property<int>("Status");

                    b.Property<int>("Storage");

                    b.Property<string>("Summary");

                    b.Property<DateTime>("UpdateTime");

                    b.Property<string>("VideoSrc");

                    b.HasKey("Id");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("Waterful.Core.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreateTime");

                    b.Property<int>("CreateUser");

                    b.Property<string>("Email");

                    b.Property<DateTime>("LoginTime");

                    b.Property<string>("Mobile");

                    b.Property<string>("Name");

                    b.Property<string>("Password")
                        .HasMaxLength(30);

                    b.Property<string>("Remark");

                    b.Property<int>("Status");

                    b.Property<DateTime>("UpdateTime");

                    b.Property<string>("UserName")
                        .HasMaxLength(30);

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Waterful.Core.Models.Worker", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreateTime");

                    b.Property<string>("Logo");

                    b.Property<string>("Mobile");

                    b.Property<string>("Name");

                    b.Property<string>("Remark");

                    b.Property<int>("Status");

                    b.Property<DateTime>("UpdateTime");

                    b.Property<string>("WorkerNo");

                    b.HasKey("Id");

                    b.ToTable("Workers");
                });

            modelBuilder.Entity("Waterful.Core.Models.Address", b =>
                {
                    b.HasOne("Waterful.Core.Models.Customer", "Customer")
                        .WithMany("Addresses")
                        .HasForeignKey("CusomerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Waterful.Core.Models.Aftersale", b =>
                {
                    b.HasOne("Waterful.Core.Models.Worker", "Worker")
                        .WithMany()
                        .HasForeignKey("WorkerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Waterful.Core.Models.OrderItem", b =>
                {
                    b.HasOne("Waterful.Core.Models.Order", "Order")
                        .WithMany("OrderItems")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Waterful.Core.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
