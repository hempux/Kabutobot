﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using net.hempux.kabuto.database;

#nullable disable

namespace net.hempux.kabuto.Migrations
{
    [DbContext(typeof(TeamsBotDbContext))]
    [Migration("20230105201722_NewNinjaTables3")]
    partial class NewNinjaTables3
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.10");

            modelBuilder.Entity("net.hempux.kabuto.database.DeviceModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("OrganizationId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SystemName")
                        .HasColumnType("TEXT");

                    b.Property<int>("approvalStatus")
                        .HasColumnType("INTEGER");

                    b.Property<string>("dnsName")
                        .HasColumnType("TEXT");

                    b.Property<string>("organizationName")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("OrganizationId");

                    b.ToTable("Devices");
                });

            modelBuilder.Entity("net.hempux.kabuto.database.OauthModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Expires_at")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Refresh_token")
                        .HasColumnType("TEXT");

                    b.Property<string>("Token")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Oauth");
                });

            modelBuilder.Entity("net.hempux.kabuto.database.OrganizationModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ApprovalMode")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Organizations");
                });

            modelBuilder.Entity("net.hempux.kabuto.database.PersistentdataModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Key")
                        .HasColumnType("TEXT");

                    b.Property<string>("Value")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Key")
                        .IsUnique();

                    b.ToTable("Persistentdata");
                });

            modelBuilder.Entity("net.hempux.kabuto.database.DeviceModel", b =>
                {
                    b.HasOne("net.hempux.kabuto.database.OrganizationModel", "Organization")
                        .WithMany("Devices")
                        .HasForeignKey("OrganizationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Organization");
                });

            modelBuilder.Entity("net.hempux.kabuto.database.OrganizationModel", b =>
                {
                    b.Navigation("Devices");
                });
#pragma warning restore 612, 618
        }
    }
}
